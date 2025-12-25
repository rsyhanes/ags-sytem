import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';

interface Order {
  id: string;
  customerName: string;
  systemCode: string;
  status: 'New' | 'In Progress' | 'Complete';
  orderDate: string;
  totalValue: number;
}

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, MatButtonModule, MatIconModule, MatChipsModule],
  template: `
    <div class="orders">
      <h1 class="orders-title">Orders</h1>
      <p class="orders-subtitle">Manage customer orders and fulfillment</p>

      <mat-card class="orders-card">
        <mat-card-content>
          <table mat-table [dataSource]="orders" class="orders-table">
            <ng-container matColumnDef="id">
              <th mat-header-cell *matHeaderCellDef>Order ID</th>
              <td mat-cell *matCellDef="let order">{{ order.id }}</td>
            </ng-container>

            <ng-container matColumnDef="customerName">
              <th mat-header-cell *matHeaderCellDef>Customer</th>
              <td mat-cell *matCellDef="let order">{{ order.customerName }}</td>
            </ng-container>

            <ng-container matColumnDef="systemCode">
              <th mat-header-cell *matHeaderCellDef>System</th>
              <td mat-cell *matCellDef="let order">{{ order.systemCode }}</td>
            </ng-container>

            <ng-container matColumnDef="status">
              <th mat-header-cell *matHeaderCellDef>Status</th>
              <td mat-cell *matCellDef="let order">
                <mat-chip [color]="getStatusColor(order.status)" selected>
                  {{ order.status }}
                </mat-chip>
              </td>
            </ng-container>

            <ng-container matColumnDef="orderDate">
              <th mat-header-cell *matHeaderCellDef>Order Date</th>
              <td mat-cell *matCellDef="let order">{{ order.orderDate }}</td>
            </ng-container>

            <ng-container matColumnDef="totalValue">
              <th mat-header-cell *matHeaderCellDef>Total Value</th>
              <td mat-cell *matCellDef="let order">\${{ order.totalValue.toFixed(2) }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let order">
                <button mat-icon-button color="primary" title="View Details">
                  <mat-icon>visibility</mat-icon>
                </button>
              </td>
            </ng-container>

            <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
            <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
          </table>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .orders {
      padding: 24px;
    }

    .orders-title {
      font-size: 32px;
      font-weight: 300;
      margin-bottom: 8px;
      color: #333;
    }

    .orders-subtitle {
      font-size: 16px;
      color: #666;
      margin-bottom: 32px;
    }

    .orders-card {
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .orders-table {
      width: 100%;
      border-collapse: collapse;
    }

    .mat-mdc-table {
      background: transparent;
    }

    .mat-mdc-header-cell {
      font-weight: 600;
      color: #333;
      border-bottom: 2px solid #e0e0e0;
    }

    .mat-mdc-cell {
      border-bottom: 1px solid #f0f0f0;
    }

    .mat-mdc-chip {
      font-size: 12px;
      min-height: 24px;
    }
  `]
})
export class OrdersComponent {
  displayedColumns: string[] = ['id', 'customerName', 'systemCode', 'status', 'orderDate', 'totalValue', 'actions'];

  orders: Order[] = [
    {
      id: 'ORD-001',
      customerName: 'John Smith',
      systemCode: 'WF00',
      status: 'Complete',
      orderDate: '2025-12-15',
      totalValue: 1247.50
    },
    {
      id: 'ORD-002',
      customerName: 'Sarah Johnson',
      systemCode: 'DF00',
      status: 'In Progress',
      orderDate: '2025-12-18',
      totalValue: 892.00
    },
    {
      id: 'ORD-003',
      customerName: 'Mike Davis',
      systemCode: 'RF00',
      status: 'New',
      orderDate: '2025-12-20',
      totalValue: 2156.75
    },
    {
      id: 'ORD-004',
      customerName: 'Lisa Brown',
      systemCode: 'WF00',
      status: 'In Progress',
      orderDate: '2025-12-19',
      totalValue: 756.25
    }
  ];

  getStatusColor(status: string): string {
    switch (status) {
      case 'New': return 'primary';
      case 'In Progress': return 'accent';
      case 'Complete': return 'basic';
      default: return 'basic';
    }
  }
}
