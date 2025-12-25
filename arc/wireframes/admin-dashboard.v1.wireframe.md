# Admin Dashboard Wireframe - v1

## Overview

Low-to-medium fidelity wireframe for the AGS Windows and Doors Admin Portal dashboard. Designed for standard desktop web application screens with clear visual hierarchy and component placement.

**Business Context:** Internal management system for MVP operations including:
- Pre-configured systems catalog (read-only)
- Inventory management for parts and components
- Order processing and status management
- Basic customer information management
- Dashboard overview of key metrics

## Layout Specifications

### Overall Structure
- **Grid System:** 12-column responsive grid
- **Breakpoint:** Desktop-first design (min-width: 1200px)
- **Color Scheme:** Grayscale only (#000000, #FFFFFF, #F5F5F5, #E0E0E0, #CCCCCC)
- **Typography:** Placeholder - system fonts

### Header Bar
- **Height:** 60px
- **Background:** Light gray (#F5F5F5)
- **Border:** 1px solid #E0E0E0 (bottom)
- **Components:**
  - Logo (left): 40x40px rectangle with "AGS" text placeholder
  - Search Bar (center): 400px wide input field with search icon (magnifying glass)
  - Actions (right):
    - Notification Bell: 24x24px icon with optional badge
    - User Profile: Circular avatar (32px) + dropdown arrow

### Sidebar Navigation
- **Width:** 250px (fixed)
- **Height:** Full viewport height minus header
- **Background:** White (#FFFFFF)
- **Border:** 1px solid #E0E0E0 (right)
- **Navigation Items:**
  - Dashboard (active/highlighted - gray background #F5F5F5)
  - Systems (pre-configured systems catalog)
  - Items (inventory management)
  - Orders (order processing)
  - Users (customer management)
- **Item Style:** 48px height, left-aligned text with 16px left padding, hover states

### Main Content Area
- **Width:** Remaining space (viewport width - 250px)
- **Height:** Full viewport height minus header
- **Background:** Light gray (#F5F5F5)
- **Padding:** 24px
- **Content:** Placeholder for dashboard widgets/cards (to be implemented)
  - Grid layout for metric cards
  - Charts/graphs placeholders
  - Recent activity feeds
  - Quick action buttons

## ASCII Art Diagram

```
+-----------------------------------------------------------+
| HEADER BAR (60px)                                         |
| +-------+ +-------------------+ +-----+ +----------------+ |
| | [AGS] | | [Search Bar     ] | | [🔔] | | [Avatar ▼]     | |
| +-------+ +-------------------+ +-----+ +----------------+ |
+-----------------------------------------------------------+
| SIDEBAR (250px) | MAIN CONTENT AREA                      |
|                 |                                         |
| 🏠 Dashboard    | +-------------------------------------+ |
| 🏗️ Systems      | | DASHBOARD CONTENT PLACEHOLDER       | |
| 📦 Items        | |                                     | |
| 📋 Orders       | | [Widget Grid Layout]               | |
| 👤 Users        | |                                     | |
|                 | | - Total Orders (metric)             | |
|                 | | - Pending Orders (metric)           | |
|                 | | - Recent Orders (table/list)        | |
|                 | | - System Usage Stats (chart)        | |
|                 | +-------------------------------------+ |
|                 |                                         |
|                 |                                         |
|                 |                                         |
|                 |                                         |
|                 |                                         |
+-----------------------------------------------------------+
```

## Responsive Behavior
- **Desktop (1200px+):** Full layout as described
- **Tablet (768-1199px):** Collapsible sidebar with hamburger menu
- **Mobile (<768px):** Top navigation, stacked layout

## Component Hierarchy (Angular Implementation Notes)

```
app/
├── core/
│   ├── layout/
│   │   ├── header/
│   │   │   ├── header.component.ts
│   │   │   ├── logo.component.ts
│   │   │   ├── search-bar.component.ts
│   │   │   ├── notifications.component.ts
│   │   │   └── user-menu.component.ts
│   │   └── sidebar/
│   │       ├── sidebar.component.ts
│   │       └── nav-item.component.ts
│   └── shell/
│       └── shell.component.ts (main layout wrapper)
├── features/
│   ├── dashboard/
│   │   ├── dashboard.component.ts
│   │   ├── widgets/
│   │   │   ├── metric-card.component.ts
│   │   │   ├── chart.component.ts
│   │   │   └── activity-feed.component.ts
│   │   └── dashboard.state.ts
│   ├── systems/
│   │   ├── systems-list.component.ts
│   │   ├── system-detail.component.ts
│   │   └── systems.state.ts
│   ├── items/
│   │   ├── items-list.component.ts
│   │   ├── item-detail.component.ts
│   │   └── items.state.ts
│   ├── orders/
│   │   ├── orders-list.component.ts
│   │   ├── order-detail.component.ts
│   │   └── orders.state.ts
│   └── users/
│       ├── users-list.component.ts
│       ├── user-detail.component.ts
│       └── users.state.ts
```

## Angular Material Implementation Notes

- Use `MatToolbar` for header
- `MatSidenav` for sidebar navigation
- `MatIcon` for icons (bell, search, etc.)
- `MatMenu` for user dropdown
- `MatCard` for dashboard widgets
- `MatGridList` for responsive grid layout

## Accessibility Considerations

- Semantic HTML structure
- ARIA labels for icons
- Keyboard navigation support
- Color contrast ratios maintained
- Focus indicators for interactive elements

## Future Implementation Notes

- Dashboard content to be implemented as feature module
- State management using NgRx (following angular.state-management rule pack)
- Responsive breakpoints to be refined based on user testing
- Color scheme to be replaced with brand colors in high-fidelity design
