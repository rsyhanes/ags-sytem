# Client Portal Wireframe - v1

## Overview

Low-to-medium fidelity wireframe for the AGS Windows and Doors Customer Portal. Customer-facing interface for placing orders on pre-configured window/door systems with automatic BOM generation. Designed as a responsive single-page application focused on order placement.

**Business Context:** Customer portal for MVP operations including:
- Order placement on existing systems with automatic BOM calculation
- Customer information collection
- Order history and status tracking

## Layout Specifications

### Overall Structure
- **Grid System:** 12-column responsive grid
- **Breakpoint:** Mobile-first design (320px+)
- **Color Scheme:** Grayscale only (#000000, #FFFFFF, #F5F5F5, #E0E0E0, #CCCCCC)
- **Typography:** Placeholder - system fonts

### Top Navigation Header
- **Height:** 60px
- **Background:** White (#FFFFFF)
- **Border:** 1px solid #E0E0E0 (bottom)
- **Components:**
  - Logo (left): 40x40px rectangle with "AGS" text placeholder
  - Navigation Menu (center): Horizontal links
    - Place Order (default/highlighted)
    - My Orders
  - Actions (right):
    - User menu (Login/Register)

### Main Content Area
- **Width:** Full viewport width
- **Height:** Full viewport height minus header
- **Background:** Light gray (#F5F5F5)
- **Padding:** 24px (responsive)
- **Content:** Dynamic based on route
  - Place Order (default): Order form with system selection, specifications, BOM display
  - My Orders: Order history table

## ASCII Art Diagram

```
+-----------------------------------------------------------+
| TOP NAV HEADER (60px)                                     |
| +-------+ +---------------------+ +-------------+         |
| | [AGS] | | Place Order | My Orders | | [Login] |         |
| +-------+ +---------------------+ +-------------+         |
+-----------------------------------------------------------+
| MAIN CONTENT AREA                                         |
|                                                           |
| +-------------------------------------------------------+ |
| | ORDER PLACEMENT FORM                                   | |
| |                                                       | |
| | System: [WF00 - Window System ▼]                      | |
| | Width: [120] inches   Height: [60] inches             | |
| | Color: [Bronze ▼]     Quantity: [1]                   | |
| |                                                       | |
| | [GENERATE BOM]                                        | |
| |                                                       | |
| | BOM PREVIEW:                                          | |
| | - Frame Rail: 120" @ $5.50 = $660.00                 | |
| | - Mullion: 48" @ $3.25 = $156.00                     | |
| | - Glass Panel: 48" @ $12.00 = $576.00                | |
| | TOTAL: $1,392.00                                      | |
| |                                                       | |
| | Customer Info:                                        | |
| | Name: [John Doe]    Email: [john@example.com]        | |
| | Phone: [555-1234]   Address: [123 Main St]           | |
| |                                                       | |
| | [PLACE ORDER]                                         | |
| +-------------------------------------------------------+ |
|                                                           |
+-----------------------------------------------------------+
```



## Responsive Behavior
- **Mobile (<768px):** Stacked navigation, single column content
- **Tablet (768-1199px):** Condensed navigation, 2-column grids
- **Desktop (1200px+):** Full navigation, multi-column layouts

## Routes, Components & State

### Routes
- `/` (place order - default)
- `/orders` (order history)
- `/orders/:id` (order details)

### Components
```
app/
├── core/
│   ├── layout/
│   │   ├── header/
│   │   │   ├── header.component.ts
│   │   │   ├── logo.component.ts
│   │   │   └── nav-menu.component.ts
│   └── shell/
│       └── shell.component.ts
├── features/
│   ├── place-order/
│   │   ├── place-order-form.component.ts
│   │   ├── system-selector.component.ts
│   │   ├── specifications-form.component.ts
│   │   ├── bom-preview.component.ts
│   │   ├── customer-info-form.component.ts
│   │   └── order.state.ts
│   └── orders/
│       ├── order-history.component.ts
│       ├── order-detail.component.ts
│       └── orders.state.ts
```

### State (NgRx)
- `order.state` (current order form data, BOM calculation, submission)
- `orders.state` (order history)

## Angular Material Implementation Notes

- Use `MatToolbar` for header navigation
- `MatFormField` with `MatInput` for dimensions/quantity
- `MatSelect` for dropdowns (system, color)
- `MatButton` for actions (Generate BOM, Place Order)
- `MatCard` for BOM preview section
- `MatTable` for order history
- `MatSidenav` for mobile navigation

## User Flow

1. **Place Order** (default route) - Fill out order form:
   - Select system from dropdown
   - Enter dimensions (width, height)
   - Select color from system's available options
   - Enter quantity
   - Generate BOM automatically
   - Enter customer contact information
   - Submit order
2. **View Order History** (`/orders`) - See past orders with status

## Accessibility Considerations

- Form validation with error messages and ARIA labels
- Keyboard navigation through form fields
- Screen reader support for BOM calculation results
- Focus management for form submission
- Color contrast ratios maintained

## Future Implementation Notes

- Real-time BOM calculation via API integration
- Order status updates with email notifications
- Saved configurations for returning customers
- Mobile-optimized stepper for small screens
- Integration with payment processing (future)
