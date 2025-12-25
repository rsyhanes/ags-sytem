import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

interface User {
  id: string;
  name: string;
  email: string;
  phone: string;
  totalOrders: number;
  lastOrderDate: string;
}

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, MatTableModule, MatCardModule, MatButtonModule, MatIconModule],
  template: `
    <div class="users">
      <h1 class="users-title">Users</h1>
      <p class="users-subtitle">Customer information and order history</p>

      <mat-card class="users-card">
        <mat-card-content>
          <table mat-table [dataSource]="users" class="users-table">
            <ng-container matColumnDef="id">
              <th mat-header-cell *matHeaderCellDef>ID</th>
              <td mat-cell *matCellDef="let user">{{ user.id }}</td>
            </ng-container>

            <ng-container matColumnDef="name">
              <th mat-header-cell *matHeaderCellDef>Name</th>
              <td mat-cell *matCellDef="let user">{{ user.name }}</td>
            </ng-container>

            <ng-container matColumnDef="email">
              <th mat-header-cell *matHeaderCellDef>Email</th>
              <td mat-cell *matCellDef="let user">{{ user.email }}</td>
            </ng-container>

            <ng-container matColumnDef="phone">
              <th mat-header-cell *matHeaderCellDef>Phone</th>
              <td mat-cell *matCellDef="let user">{{ user.phone }}</td>
            </ng-container>

            <ng-container matColumnDef="totalOrders">
              <th mat-header-cell *matHeaderCellDef>Total Orders</th>
              <td mat-cell *matCellDef="let user">{{ user.totalOrders }}</td>
            </ng-container>

            <ng-container matColumnDef="lastOrderDate">
              <th mat-header-cell *matHeaderCellDef>Last Order</th>
              <td mat-cell *matCellDef="let user">{{ user.lastOrderDate }}</td>
            </ng-container>

            <ng-container matColumnDef="actions">
              <th mat-header-cell *matHeaderCellDef>Actions</th>
              <td mat-cell *matCellDef="let user">
                <button mat-icon-button color="primary" title="View Details">
                  <mat-icon>visibility</mat-icon>
                </button>
                <button mat-icon-button color="accent" title="Contact">
                  <mat-icon>email</mat-icon>
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
    .users {
      padding: 24px;
    }

    .users-title {
      font-size: 32px;
      font-weight: 300;
      margin-bottom: 8px;
      color: #333;
    }

    .users-subtitle {
      font-size: 16px;
      color: #666;
      margin-bottom: 32px;
    }

    .users-card {
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .users-table {
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
  `]
})
export class UsersComponent {
  displayedColumns: string[] = ['id', 'name', 'email', 'phone', 'totalOrders', 'lastOrderDate', 'actions'];

  users: User[] = [
    {
      id: 'USR-001',
      name: 'John Smith',
      email: 'john.smith@example.com',
      phone: '(555) 123-4567',
      totalOrders: 3,
      lastOrderDate: '2025-12-15'
    },
    {
      id: 'USR-002',
      name: 'Sarah Johnson',
      email: 'sarah.j@example.com',
      phone: '(555) 234-5678',
      totalOrders: 1,
      lastOrderDate: '2025-12-18'
    },
    {
      id: 'USR-003',
      name: 'Mike Davis',
      email: 'mike.davis@example.com',
      phone: '(555) 345-6789',
      totalOrders: 5,
      lastOrderDate: '2025-12-20'
    },
    {
      id: 'USR-004',
      name: 'Lisa Brown',
      email: 'lisa.brown@example.com',
      phone: '(555) 456-7890',
      totalOrders: 2,
      lastOrderDate: '2025-12-19'
    }
  ];
}
