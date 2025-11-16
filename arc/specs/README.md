# AGS Windows & Doors MVP - Feature Specifications

## Overview

This folder contains feature-based specifications for the AGS Windows & Doors MVP system, following spec-driven development principles. Each specification represents a discrete feature with clear domain boundaries, ports, and contracts.

## Architecture Approach

- **Domain Purity**: Core business logic free from framework dependencies
- **Ports & Adapters**: Clean interfaces between layers
- **Contract-First**: API and port contracts defined upfront
- **Test-Driven**: Scenarios and deliverables include comprehensive testing

## MVP Features

### Catalog Management
- `items.manage-catalog-item.v1` - Create and manage building components catalog

### System Design  
- `systems.design-system.v1` - Create window/door/railing system definitions
- `systems.configure-components.v1` - Define component relationships and calculations

### Order Processing
- `orders.calculate-bom.v1` - Calculate bill of materials from frame specifications  
- `orders.submit-order.v1` - Process customer orders with validation

## Domain Model Foundation

The specifications leverage the AGS domain model with these core entities:
- **Item**: Catalog components (Frame, Glass Stop, etc.)
- **System**: Product templates (Window, Door, Railing)
- **SystemComponent**: Component configuration with dynamic calculations
- **Frame**: Physical specifications for manufacturing
- **Order**: Customer requests with calculated BOMs

## Common Packs Used

- `domain.purity` - Framework-free domain logic
- `ports.contracts` - Versioned interface definitions
- `adapters.conformance` - Implementation compliance
- `web-api` - HTTP/OpenAPI interfaces
- `persistence` - Repository patterns
- `calculation-engine` - Formula processing

## Usage

Each spec is implementation-ready and includes:
- Clear objectives and scope
- Domain entities and business rules
- Port contracts (driving/driven)
- Non-functional requirements
- Test scenarios and edge cases
- Complete deliverables list

Follow the specs in dependency order or implement incrementally based on business priority.
