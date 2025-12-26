import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatChipSet } from '@angular/material/chips';
import { MatDialogModule, MatDialog } from '@angular/material/dialog';

import { ComponentsStore } from '../data-access/components.state';

@Component({
  selector: 'app-components-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatProgressSpinnerModule,
    MatCardModule,
    MatChipsModule,
    MatDialogModule
  ],
  template: `
    <div class="page-container">
      <mat-toolbar class="toolbar" color="primary">
        <h1>System {{ systemCode() }} Components</h1>
        <button mat-raised-button color="accent" (click)="navigateToNew()">
          <mat-icon>add</mat-icon>
          Add Component
        </button>
      </mat-toolbar>

      @if (loading(); as isLoading) {
        <div class="loading-container">
          <mat-spinner diameter="40"></mat-spinner>
          <p>Loading components...</p>
        </div>
      } @else if (error(); as errorMessage) {
        <mat-card class="error-card">
          <mat-card-content>
            <h3>Error Loading Components</h3>
            <p>{{ errorMessage }}</p>
            <button mat-button (click)="retryLoad()">
              <mat-icon>refresh</mat-icon>
              Retry
            </button>
          </mat-card-content>
        </mat-card>
      } @else {
        <div class="content">
          @if (components(); as componentsData) {
            @if (componentsData.length === 0) {
              <mat-card class="empty-state">
                <mat-card-content>
                  <mat-icon class="empty-icon">build</mat-icon>
                  <h3>No components configured</h3>
                  <p>This system doesn't have any components yet.</p>
                  <button mat-raised-button color="accent" (click)="navigateToNew()">
                    <mat-icon>add</mat-icon>
                    Add First Component
                  </button>
                </mat-card-content>
              </mat-card>
            } @else {
              <div class="components-summary">
                <mat-chip>{{ componentsData.length }} Components</mat-chip>
                <mat-chip color="accent">{{ requiredComponents().length }} Required</mat-chip>
                <mat-chip color="warn">{{ optionalComponents().length }} Optional</mat-chip>
              </div>

              <table mat-table [dataSource]="componentsData" class="components-table">
                <!-- Sort Order Column -->
                <ng-container matColumnDef="sortOrder">
                  <th mat-header-cell *matHeaderCellDef>Order</th>
                  <td mat-cell *matCellDef="let component">{{ component.sortOrder }}</td>
                </ng-container>

                <!-- Item Code Column -->
                <ng-container matColumnDef="itemCode">
                  <th mat-header-cell *matHeaderCellDef>Item Code</th>
                  <td mat-cell *matCellDef="let component">{{ component.itemCode }}</td>
                </ng-container>

                <!-- Name Column -->
                <ng-container matColumnDef="name">
                  <th mat-header-cell *matHeaderCellDef>Name</th>
                  <td mat-cell *matCellDef="let component">{{ component.name }}</td>
                </ng-container>

                <!-- Quantity Column -->
                <ng-container matColumnDef="quantity">
                  <th mat-header-cell *matHeaderCellDef>Quantity</th>
                  <td mat-cell *matCellDef="let component">{{ component.quantity }}</td>
                </ng-container>

                <!-- Dimensions Column -->
                <ng-container matColumnDef="dimensions">
                  <th mat-header-cell *matHeaderCellDef>Dimensions</th>
                  <td mat-cell *matCellDef="let component">
                    @if (component.dimensions?.lengthFormula) {
                      Formula: {{ component.dimensions.lengthFormula }}
                    } @else if (component.dimensions?.fixedLength) {
                      {{ component.dimensions.fixedLength.value }} {{ component.dimensions.fixedLength.unit }}
                    } @else {
                      <em>No dimensions</em>
                    }
                  </td>
                </ng-container>

                <!-- Required Column -->
                <ng-container matColumnDef="isRequired">
                  <th mat-header-cell *matHeaderCellDef>Required</th>
                  <td mat-cell *matCellDef="let component">
                    <mat-icon [class.required]="component.isRequired" [class.optional]="!component.isRequired">
                      {{ component.isRequired ? 'check_circle' : 'radio_button_unchecked' }}
                    </mat-icon>
                  </td>
                </ng-container>

                <!-- Actions Column -->
                <ng-container matColumnDef="actions">
                  <th mat-header-cell *matHeaderCellDef>Actions</th>
                  <td mat-cell *matCellDef="let component">
                    <button mat-icon-button [attr.aria-label]="'Edit ' + component.name" (click)="editComponent(component.id)">
                      <mat-icon>edit</mat-icon>
                    </button>
                    <button mat-icon-button color="warn" [attr.aria-label]="'Delete ' + component.name" (click)="deleteComponent(component)">
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

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 200px;
      gap: 16px;
    }

    .error-card {
      margin: 24px;
      text-align: center;
    }

    .content {
      flex: 1;
      padding: 24px;
      overflow-y: auto;
    }

    .components-summary {
      margin-bottom: 24px;
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

    .components-table {
      width: 100%;
    }

    .required {
      color: var(--mat-sys-primary);
    }

    .optional {
      color: var(--mat-sys-outline);
    }

    button {
      margin-left: 4px;
    }
  `]
})
export class ComponentsListPage {
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);
  private componentsStore = inject(ComponentsStore);

  systemCode = signal<string>('');

  displayedColumns = ['sortOrder', 'itemCode', 'name', 'quantity', 'dimensions', 'isRequired', 'actions'];

  // Reactive state bindings
  components = this.componentsStore.components;
  loading = this.componentsStore.loading;
  error = this.componentsStore.error;
  requiredComponents = this.componentsStore.requiredComponents;
  optionalComponents = this.componentsStore.optionalComponents;

  constructor() {
    // Get system code from route params
    const systemCode = this.route.snapshot.params['systemCode'];
    this.systemCode.set(systemCode);

    // Load components for this system
    this.componentsStore.loadComponents(systemCode);
  }

  navigateToNew() {
    this.router.navigate(['new'], { relativeTo: this.route });
  }

  editComponent(id: string) {
    this.router.navigate([id, 'edit'], { relativeTo: this.route });
  }

  deleteComponent(component: any) {
    // TODO: Implement delete with confirmation dialog
    console.log('Delete component:', component.id);
  }

  retryLoad() {
    this.componentsStore.clearError();
    this.componentsStore.loadComponents(this.systemCode());
  }
}
