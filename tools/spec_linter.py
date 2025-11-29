#!/usr/bin/env python3
"""
Spec Linter with Spec-Specific Feedback
---------------------------------------

Validates and scores specs for completeness, traceability,
reference integrity, and behavioral coverage — with actionable hints.

Usage:
    python tools/spec_linter.py arc/specs
"""

import json
import yaml
import sys
from pathlib import Path
from jsonschema import validate, ValidationError

BASE_PATH = Path(__file__).resolve().parents[1]
SPEC_SCHEMA_PATH = BASE_PATH / "arc/rules/schemas/spec.schema.json"

# ---------- Utilities ----------

def load_yaml(file_path):
    with open(file_path, "r", encoding="utf-8") as f:
        return yaml.safe_load(f)

def file_exists(path):
    return path.exists() and path.is_file()

def badge(score):
    if score >= 90: return "🟢 Excellent"
    if score >= 75: return "🟡 Good"
    if score >= 50: return "🟠 Needs Work"
    return "🔴 Incomplete"

def hint(section):
    """Quick hints for missing sections."""
    hints = {
        "id": "Add a unique 'id' (e.g., context.feature.v1).",
        "objective": "Describe the intent in one sentence.",
        "context": "Specify the bounded context (e.g., users, orders).",
        "contracts": "Add 'contracts' referencing OpenAPI/AsyncAPI or schema paths.",
        "scope": "Include a 'scope' section defining domain, in, and out elements.",
        "scope.domain": "Define at least one Entity or UseCase.",
        "scope.in": "Add an entry point (e.g., UI route or HTTP endpoint).",
        "scope.out": "Add an outcome (e.g., Event, Email, Persistence).",
        "scenarios": "Define at least one Given/When/Then scenario.",
        "deliverables": "List expected outputs (domain entities, ports, adapters, tests).",
        "packs": "Include at least one rule pack ID (e.g., domain.purity).",
    }
    return hints.get(section, "")

def print_result(path, score, messages, missing_summary):
    print(f"\n{path.name}")
    print("-" * len(path.name))
    for msg in messages:
        print(msg)
    print(f"Score: {score}/100 ({badge(score)})")
    if missing_summary:
        print("\n🪄 Suggestions:")
        for m in missing_summary:
            print(f"   - {m}")
    print("=" * 60)

# ---------- Checks ----------

def check_structural(spec):
    required = ["id","objective","context","contracts","scope","scenarios","deliverables","packs"]
    missing = [r for r in required if r not in spec]
    messages, suggestions = [], []
    score = 40
    if missing:
        for r in missing:
            messages.append(f"⚠ Missing section: {r} (-10pts)")
            suggestions.append(f"{r}: {hint(r)}")
        score -= len(missing) * 10
    else:
        messages.append("✔ Structural completeness OK")
    return max(0, score), messages, suggestions

def check_traceability(spec):
    s = spec.get("scope", {})
    messages, suggestions = [], []
    score = 25
    if not (s.get("in") and s.get("domain") and s.get("out")):
        messages.append("❌ Missing one of scope.in/domain/out (-25pts)")
        if not s.get("in"): suggestions.append("scope.in: " + hint("scope.in"))
        if not s.get("domain"): suggestions.append("scope.domain: " + hint("scope.domain"))
        if not s.get("out"): suggestions.append("scope.out: " + hint("scope.out"))
        return 0, messages, suggestions
    if not any("HTTP:" in i or "UI:" in i for i in s.get("in", [])):
        messages.append("⚠ No driving input (UI/API) found (-5pts)")
        suggestions.append("Add at least one 'UI:' or 'HTTP:' entry in scope.in.")
        score -= 5
    if not any(any(k in o for k in ["Event:", "Persistence:", "Email:", "API:"]) for o in s.get("out", [])):
        messages.append("⚠ No external output found (-5pts)")
        suggestions.append("Add an 'Event:', 'Persistence:', or 'Email:' entry in scope.out.")
        score -= 5
    messages.append("✔ Vertical trace OK")
    return max(0, score), messages, suggestions

def check_references(spec):
    messages, suggestions = [], []
    score = 20
    missing_refs = 0
    contracts = spec.get("contracts", {})
    packs = spec.get("packs", [])

    def check_path(p):
        nonlocal missing_refs
        target = BASE_PATH / p.split('#')[0]
        if not file_exists(target):
            messages.append(f"❌ Missing contract file: {p} (-10pts)")
            suggestions.append(f"Add or correct contract path: {p}")
            missing_refs += 1

    for v in contracts.values():
        if isinstance(v, list):
            for p in v: check_path(p)
        elif isinstance(v, str):
            check_path(v)

    for pack in packs:
        pack_path = BASE_PATH / "arc/rules/packs" / f"{pack}.yaml"
        if not file_exists(pack_path):
            messages.append(f"❌ Missing rule pack: {pack} (-10pts)")
            suggestions.append(f"Ensure '{pack}.yaml' exists under arc/rules/packs.")
            missing_refs += 1

    score -= missing_refs * 10
    if missing_refs == 0:
        messages.append("✔ References OK")
    return max(0, score), messages, suggestions

def check_behavior(spec):
    messages, suggestions = [], []
    score = 15
    s = spec.get("scope", {})
    inputs = s.get("in", [])
    outputs = s.get("out", [])
    scenario_text = " ".join(json.dumps(spec.get("scenarios", [])))

    uncovered = 0
    for i in inputs:
        label = i.split(":", 1)[-1].strip()
        if label and label not in scenario_text:
            messages.append(f"⚠ Input '{label}' not covered (-2pts)")
            suggestions.append(f"Add a scenario using input '{label}'.")
            score -= 2
            uncovered += 1
    for o in outputs:
        label = o.split(":", 1)[-1].strip()
        if label and label not in scenario_text:
            messages.append(f"⚠ Output '{label}' not covered (-2pts)")
            suggestions.append(f"Add a scenario verifying output '{label}'.")
            score -= 2
            uncovered += 1
    if uncovered == 0:
        messages.append("✔ Behavior coverage OK")
    return max(0, score), messages, suggestions

# ---------- Main Lint ----------

def lint_spec(path):
    try:
        data = load_yaml(path)
        spec = data.get("spec", {})
    except Exception as e:
        return 0, [f"❌ Cannot parse YAML: {e}"], [f"Check YAML syntax in {path.name}."]

    try:
        schema = load_yaml(SPEC_SCHEMA_PATH)
        validate(instance=data, schema=schema)
    except ValidationError as e:
        return 0, [f"❌ Schema validation failed: {e.message}"], [f"Fix structure per spec.schema.json."]

    total = 0
    messages, suggestions = [], []

    for check in [check_structural, check_traceability, check_references, check_behavior]:
        pts, msgs, hints = check(spec)
        total += pts
        messages.extend(msgs)
        suggestions.extend(hints)

    return round(total), messages, suggestions

def main():
    if len(sys.argv) < 2:
        print("Usage: python tools/spec_linter.py <specs_dir>")
        sys.exit(1)

    specs_dir = Path(sys.argv[1])
    if not specs_dir.exists():
        print(f"Directory not found: {specs_dir}")
        sys.exit(1)

    print(f"🔍 Running Spec Linter with feedback on {specs_dir}...\n")

    total_score = 0
    count = 0
    for path in specs_dir.glob("*.spec.yaml"):
        score, messages, suggestions = lint_spec(path)
        print_result(path, score, messages, suggestions)
        total_score += score
        count += 1

    avg = round(total_score / count, 1) if count else 0
    print(f"\n🏁 Average Spec Quality: {avg}/100 ({badge(avg)})")
    sys.exit(0 if avg >= 75 else 1)

if __name__ == "__main__":
    main()
