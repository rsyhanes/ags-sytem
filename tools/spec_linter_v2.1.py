import sys
import yaml
from jsonschema import validate, ValidationError
from pathlib import Path

# Domain-first spec linter (v3)
# Now with JSON Schema validation (v2)
# Ensures structure, ordering, and completeness

def load_yaml(path):
    with open(path, 'r', encoding='utf-8') as f:
        return yaml.safe_load(f)


def error(msg):
    return {"ok": False, "msg": msg}


def success(msg):
    return {"ok": True, "msg": msg}


# Load schema
BASE_DIR = Path(__file__).resolve().parent.parent
SCHEMA_PATH = BASE_DIR / "arc" / "rules" / "schemas" / "spec.schema.json"
try:
    SPEC_SCHEMA = load_yaml(SCHEMA_PATH)
except Exception:
    SPEC_SCHEMA = None

def score_for(spec):
    score = 0

    required_sections = ["domain", "interactions", "contracts", "scenarios", "deliverables", "packs"]

    # 0. Schema validation
    try:
        validate(instance={"spec": spec}, schema=SPEC_SCHEMA)
    except Exception as e:
        return 0, error(f"Schema validation failed: {e}")

    # 1. Check required sections exist
    for section in required_sections:
        if section not in spec:
            return 0, error(f"Missing required section: {section}")
    score += 30

    # 2. Domain section completeness
    domain = spec.get("domain", {})
    domain_reqs = ["problem", "rationale", "use_case"]
    if all(k in domain for k in domain_reqs):
        score += 20
    else:
        return score, error("Domain section incomplete: missing problem/rationale/use_case")

    # 3. Ordering: domain must come before interactions
    keys = list(spec.keys())
    if keys.index("domain") < keys.index("interactions"):
        score += 10
    else:
        return score, error("`domain` must appear before `interactions`.")

    # 4. Contracts structure
    contracts = spec.get("contracts", {})
    required_contract_groups = ["domain", "inbound", "outbound", "ui_state"]
    if all(group in contracts for group in required_contract_groups):
        score += 15
    else:
        return score, error("Contracts must contain domain, inbound, outbound, ui_state.")

    # 5. UI state references
    ui_state = contracts.get("ui_state", {})
    if "derives_from" in ui_state and "request" in ui_state["derives_from"]:
        score += 10
    else:
        return score, error("UI state must derive from request and response contracts.")

    # 6. Scenarios present
    if spec.get("scenarios"):
        score += 10

    return min(score, 100), success("Spec looks valid.")


def lint_specs(specs_dir):
    specs = Path(specs_dir).glob("*.spec.yaml")
    results = []
    total = 0
    count = 0

    for path in specs:
        spec = load_yaml(path)
        score, result = score_for(spec)
        total += score
        count += 1
        results.append((path.name, score, result))

    avg = total / count if count else 0

    print(f"\n🔍 Domain-First Spec Linter Results for {specs_dir}\n")
    for name, score, result in results:
        status = "🟢" if score >= 80 else "🟡" if score >= 50 else "🔴"
        print(f"{status} {name}: {score}/100 → {result['msg']}")

    print(f"\n🏁 Average Score: {avg:.1f}/100\n")


if __name__ == "__main__":
    if len(sys.argv) < 2:
        print("Usage: python spec_linter.py <specs_dir>")
        sys.exit(1)
    lint_specs(sys.argv[1])
