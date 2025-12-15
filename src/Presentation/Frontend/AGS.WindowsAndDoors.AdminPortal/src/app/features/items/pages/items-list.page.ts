import { Component, inject, effect, signal } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ItemsStore } from '../data-access/items.state';
import { LoadingIndicatorComponent } from '../components/loading-indicator.component';
import { ErrorMessageComponent } from '../components/error-message.component';

@Component({
  selector: 'app-items-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    LoadingIndicatorComponent,
    ErrorMessageComponent
  ],
  template: `
    <div class="page-container">
      <mat-toolbar class="toolbar" color="primary">
        <h1>Items Catalog</h1>
        <button mat-raised-button color="accent" (click)="navigateToNew()">
          <mat-icon>add</mat-icon>
          New Item
        </button>
      </mat-toolbar>

      @if (loading(); as isLoading) {
        <app-loading-indicator />
      } @else if (error(); as errorMessage) {
        <app-error-message [message]="errorMessage" />
        <button mat-button (click)="retryLoad()">
          <mat-icon>refresh</mat-icon>
          Retry
        </button>
      } @else {
        <div class="content">
          @if (items(); as itemsData) {
            @if (itemsData.length === 0) {
              <div class="empty-state">
                <mat-icon class="empty-icon">inventory_2</mat-icon>
                <h3>No items found</h3>
                <p>Get started by creating your first item.</p>
                <button mat-raised-button color="accent" (click)="navigateToNew()">
                  <mat-icon>add</mat-icon>
                  Create First Item
                </button>
              </div>
            } @else {
              <table mat-table [dataSource]="itemsData" class="items-table">
                <!-- Code Column -->
                <ng-container matColumnDef="code">
                  <th mat-header-cell *matHeaderCellDef>Code</th>
                  <td mat-cell *matCellDef="let item">{{ item.code }}</td>
                </ng-container>

                <!-- Name Column -->
                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef>Name</th>
                  <td mat-cell *matCellDef="let item">{{ item.name }}</td>
                </ng-container>

                <!-- Price Column -->
                <ng-container matColumnDef="price">
                  <th mat-header-cell *matHeaderCellDef>Price</th>
                  <td mat-cell *matCellDef="let item">{{ item.price | currency }}</td>
                </ng-container>

                <!-- Measure Column -->
                <ng-container matColumnDef="measure">
                  <th mat-header-cell *matHeaderCellDef>Measure</th>
                  <td mat-cell *matCellDef="let item">{{ item.dimensions?.unit || 'Units' }}</td>
                </ng-container>

                <!-- State Column -->
                <ng-container matColumnDef="state">
                  <th mat-header-cell *matHeaderCellDef>State</th>
                  <td mat-cell *matCellDef="let item">
                    <span [class]="'state-' + item.state.toLowerCase()">
                      {{ item.state }}
                    </span>
                  </td>
                </ng-container>

                <!-- Actions Column -->
                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let item">
                    <button mat-icon-button [attr.aria-label]="'Edit ' + item.name" (click)="editItem(item.code)">
                      <mat-icon>edit</mat-icon>
                    </button>
                    <button mat-icon-button color="warn" [attr.aria-label]="'Delete ' + item.name" (click)="deleteItem(item)">
                      <mat-icon>delete</mat-icon>
                    </button>
                  </td>
                </ng-container>

                <tr mat-header-row *matHeaderRowDef="displayedColumns"></tr>
                <tr mat-row *matRowDef="let row; columns: displayedColumns;"></tr>
              </table>
            }
          }
        </div>
      }
    </div>
  `,
  styles: [`
    .page-container {
      height: 100vh;
      display: flex;
      flex-direction: column;
    }

    .toolbar {
      h1 {
        margin: 0 auto 0 0;
        font-size: 24px;
        font-weight: 400;
      }
    }

    .content {
      flex: 1;
      padding: 24px;
      overflow-y: auto;
    }

    .items-table {
      width: 100%;
    }

    .empty-state {
      text-align: center;
      padding: 48px;
      max-width: 400px;
      margin: 0 auto;

      .empty-icon {
        font-size: 64px;
        width: 64px;
        height: 64px;
        opacity: 0.5;
        margin-bottom: 16px;
      }

      h3 {
        margin: 16px 0 8px 0;
      }

      p {
        margin-bottom: 24px;
        color: var(--mat-sys-on-surface-variant);
      }
    }

    .state-draft { color: var(--mat-sys-secondary); }
    .state-active { color: var(--mat-sys-primary); }
    .state-inactive { color: var(--mat-sys-error); }

    button {
      margin-left: 4px;
    }
  `]
})
export class ItemsListPage {
  private router = inject(Router);
  private itemsStore = inject(ItemsStore);

  displayedColumns = ['code', 'name', 'price', 'measure', 'state', 'actions'];

  // Reactive state bindings
  items = this.itemsStore.items;
  loading = this.itemsStore.loading;
  error = this.itemsStore.error;

  constructor() {
    // Load items on component init
    this.itemsStore.loadItems();
  }

  navigateToNew() {
    this.router.navigate(['new'], { relativeTo: this.router.routerState.root.firstChild });
  }

  editItem(code: string) {
    this.router.navigate([code, 'edit'], { relativeTo: this.router.routerState.root.firstChild });
  }

  deleteItem(item: any) {
    // TODO: Implement delete with confirmation dialog
    console.log('Delete item:', item.code);
  }

  retryLoad() {
    this.itemsStore.clearError();
    this.itemsStore.loadItems();
  }
}
