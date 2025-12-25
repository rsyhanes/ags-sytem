# AGS Windows and Doors System - Product Roadmap

## Vision
Build a spec-driven, domain-first ordering system for architectural windows and doors that enables customers to configure and order custom frames based on pre-designed system templates, with automatic Bill of Materials calculation for manufacturing accuracy.

## Core Domain Concepts
- **Systems**: Pre-designed templates defining architectural products (windows, doors, railings) with components, colors, and constraints
- **Frames**: Customer-configured instances of systems with specific dimensions, color, and quantity
- **BOM**: Automatically calculated component requirements for frame manufacturing
- **Orders**: Customer submissions containing frame configurations and calculated BOMs

## MVP Roadmap (Implementation Order)

### Phase 1: System Foundation
**Goal**: Establish the product catalog foundation

#### 1. System Catalog ⭐
**What**: Read-only browsing of pre-designed system templates
**Why**: Customers need to see available products before configuring
**Requirements**:
- List available systems (WF00-Window, DR01-Door, etc.)
- Display system details: category, available colors, size constraints
- Search/filter by category
**Spec Target**: `systems.browse-catalog.v1.spec.yaml`

### Phase 2: Configuration & Calculation
**Goal**: Enable frame configuration and automatic manufacturing planning

#### 2. Frame Configurator ⭐
**What**: UI for configuring frame instances from system templates
**Why**: Core customer workflow - specify concrete requirements
**Requirements**:
- Select system template
- Input dimensions (width/height) with constraint validation
- Choose color from system options
- Specify quantity
- Real-time form validation
**Spec Target**: `systems.configure-frame.v1.spec.yaml`

#### 3. Auto-calculation Engine ⭐
**What**: Real-time BOM generation for configured frames
**Why**: Ensure accurate component calculation for manufacturing
**Requirements**:
- Calculate components based on system + frame specs
- Display item codes, names, quantities, lengths
- Recalculate on any spec change
- Prevent invalid configs from generating BOM
**Spec Target**: `orders.calculate-bom.v1.spec.yaml` (exists, may need updates)

### Phase 3: Ordering & Management
**Goal**: Complete the order lifecycle

#### 4. Order Submission ⭐
**What**: Submit orders with frame configurations and BOMs
**Why**: Convert configurations into actionable orders
**Requirements**:
- Collect customer contact information
- Display order summary with frames and BOM details
- Validate configurations before submission
- Persist orders with notification triggers
**Spec Target**: `orders.submit-frame-order.v1.spec.yaml`

#### 5. Basic Order Management ⭐
**What**: Track and manage submitted orders
**Why**: Support order fulfillment and customer service
**Requirements**:
- List orders with pagination/filtering
- View order details (customer, frames, BOM)
- Update order status (New → In Progress → Complete)
- Search by customer or order number
**Spec Target**: `orders.manage-orders.v1.spec.yaml`

## Technical Approach
- **Spec-Driven Development**: Each feature above drives spec creation
- **Ports & Adapters Architecture**: Domain-first with clear boundaries
- **Angular Frontend + .NET Backend**: Following established tech stack
- **Domain Modeling**: Strict separation of Systems (templates) and Frames (instances)

## Success Metrics
- Customers can successfully browse systems and configure frames
- BOM calculations are accurate and update in real-time
- Orders are submitted and tracked through fulfillment
- System supports the complete ordering workflow from browse to delivery

## Future Phases (Post-MVP)
- Advanced system design tools for engineers
- Bulk ordering and quoting
- Integration with manufacturing systems
- Customer account management
- Advanced reporting and analytics
