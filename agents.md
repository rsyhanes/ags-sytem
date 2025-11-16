# Master Agent Prompt - AGS Windows and Doors System

You are Cline, working on a monorepo using spec-driven development with Ports & Adapters architecture.

## 🔄 Primary Workflow

**ALWAYS follow this sequence:**

1. **Read `/arc/index.yaml`** - Your single source of truth for:
   - Tech stack & architecture style
   - Folder conventions & layout patterns  
   - File naming & ID patterns
   - Agent operation rules

2. **Read `/arc/agent-guidelines.md`** - Implementation rules and constraints

3. **Load the target spec** from `/arc/specs/*.yaml` - Defines WHAT to build

4. **Follow the agent_flow** defined in index.yaml:
   - Resolve referenced rule packs from `/arc/rules/`
   - Resolve referenced contracts from `/arc/contracts/`
   - Plan changes respecting Ports & Adapters layout
   - Implement only aligned files/paths
   - Generate tests mapping to spec scenarios

## ✅ Quality Gates

Before completing any task:
- [ ] Follows layout defined in index.yaml
- [ ] Respects precedence order (specs → contracts → rules)
- [ ] Honors dependency rules from agent-guidelines.md
- [ ] No new top-level patterns without updating index.yaml

## 🧠 Self-Improvement & Pattern Recognition

### Pattern Documentation Protocol

When you encounter recurring implementation patterns:

1. **Identify the Pattern**
   - Note: Context, trigger, solution structure
   - Example: "When implementing aggregate roots in Domain layer..."

2. **Document in `/arc/rules/packs/`**
   - Create/update relevant rule pack (e.g., `domain.modeling.yaml`)
   - Follow existing rule pack structure
   - Include: description, constraints, examples

3. **Reference from Specs**
   - Update specs to reference new rule pack by ID
   - Remove inline rule descriptions

### Continuous Learning Triggers

**Recognize these situations for pattern extraction:**
- Repeated code structures across bounded contexts
- Common port/adapter implementation patterns  
- Consistent test organization approaches
- Recurring domain modeling decisions
- Standard integration patterns

**Pattern Extraction Process:**
```
1. Identify → 2. Abstract → 3. Document → 4. Reference → 5. Validate
```

### Self-Validation Questions

Ask yourself after each implementation:
- "Did I discover a reusable pattern?"  
- "Could this be abstracted into a rule pack?"
- "Does this pattern exist elsewhere in the codebase?"
- "Should this update the index.yaml conventions?"

### Knowledge Base Enhancement

Maintain awareness of:
- **Emerging patterns** in bounded contexts
- **Anti-patterns** that violate architecture principles  
- **Integration points** between contexts
- **Contract evolution** patterns
- **Test strategy** effectiveness

When patterns emerge consistently (3+ occurrences), propose:
1. Rule pack creation/update
2. Convention addition to index.yaml
3. Agent-guidelines.md enhancement

## 🎯 Success = Following the Flow

Your effectiveness is measured by:
- Consistent adherence to index.yaml-defined flow
- Zero deviations from established conventions  
- Proactive pattern recognition and documentation
- Continuous improvement of the `/arc` knowledge base

**Remember**: The `/arc` structure is designed to be self-documenting and self-improving. Trust the flow, enhance the patterns.
