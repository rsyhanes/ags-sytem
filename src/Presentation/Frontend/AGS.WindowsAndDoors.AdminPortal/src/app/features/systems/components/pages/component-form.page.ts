import { Component, inject, signal, computed, effect } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';

import { ComponentsStore } from '../data-access/components.state';
import { CreateComponentRequest, UpdateComponentRequest } from '../data-access/components.api';

@Component({
  selector: 'app-component-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatSlideToggleModule
  ],
  template: `
    <div class="form-container">
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>{{ isEditing() ? 'Edit Component' : 'Add Component to System' }}</mat-card-title>
          <mat-card-subtitle>System: {{ systemCode() }}</mat-card-subtitle>
        </mat-card-header>

        <mat-card-content>
          @if (loading(); as isLoading) {
            <div class="loading-container">
              <mat-spinner diameter="40"></mat-spinner>
              <p>Loading...</p>
            </div>
          } @else if (error(); as errorMessage) {
            <div class="error-message">
              <mat-icon color="warn">error</mat-icon>
              <span>{{ errorMessage }}</span>
              <button mat-button (click)="retryLoad()">Retry</button>
            </div>
          } @else {
            <form [formGroup]="componentForm" (ngSubmit)="onSubmit()" class="component-form">
              <!-- Item Code Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Item Code</mat-label>
                <mat-select formControlName="itemCode" required>
                  <mat-option value="2103">2103 - Frame</mat-option>
                  <mat-option value="2104">2104 - Glass Panel</mat-option>
                  <mat-option value="2105">2105 - Hardware Kit</mat-option>
                </mat-select>
                <mat-hint>Select the catalog item for this component</mat-hint>
                @if (componentForm.get('itemCode')?.hasError('required')) {
                  <mat-error>Item code is required</mat-error>
                }
              </mat-form-field>

              <!-- Name Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Component Name</mat-label>
                <input matInput formControlName="name" placeholder="e.g. Left Frame Vertical" required>
                <mat-hint>Descriptive name for this component</mat-hint>
                @if (componentForm.get('name')?.hasError('required')) {
                  <mat-error>Name is required</mat-error>
                }
              </mat-form-field>

              <!-- Quantity Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Quantity</mat-label>
                <input matInput type="number" min="1" formControlName="quantity" placeholder="1" required>
                <mat-hint>Number of this component needed</mat-hint>
                @if (componentForm.get('quantity')?.hasError('required')) {
                  <mat-error>Quantity is required</mat-error>
                } @else if (componentForm.get('quantity')?.hasError('min')) {
                  <mat-error>Quantity must be at least 1</mat-error>
                }
              </mat-form-field>

              <!-- Description Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Description</mat-label>
                <textarea matInput formControlName="description" rows="3"
                          placeholder="Optional description of this component"></textarea>
                <mat-hint>Additional details about this component</mat-hint>
              </mat-form-field>

              <!-- Dimensions Section -->
              <div class="dimensions-section">
                <h4>Dimensions</h4>
                <p class="dimensions-hint">Choose either a length formula or fixed dimensions</p>

                <!-- Length Formula Field -->
                <mat-form-field appearance="outline" class="field">
                  <mat-label>Length Formula</mat-label>
                  <input matInput formControlName="lengthFormula"
                         placeholder="e.g. frame.Height * 0.8">
                  <mat-hint>Formula for calculating component length (optional)</mat-hint>
                </mat-form-field>

                <!-- OR divider -->
                <div class="or-divider">
                  <span>OR</span>
                </div>

                <!-- Fixed Length Section -->
                <div class="fixed-length-row">
                  <mat-form-field appearance="outline" class="field">
                    <mat-label>Fixed Length</mat-label>
                    <input matInput type="number" step="0.01" min="0" formControlName="fixedLengthValue"
                           placeholder="0.00">
                    <mat-hint>Numeric length value</mat-hint>
                  </mat-form-field>

                  <mat-form-field appearance="outline" class="field">
                    <mat-label>Unit</mat-label>
                    <mat-select formControlName="fixedLengthUnit">
                      <mat-option value="inches">Inches</mat-option>
                      <mat-option value="feet">Feet</mat-option>
                      <mat-option value="mm">Millimeters</mat-option>
                    </mat-select>
                  </mat-form-field>
                </div>
              </div>

              <!-- Required Toggle -->
              <div class="toggle-row">
                <mat-slide-toggle formControlName="isRequired">Required Component</mat-slide-toggle>
                <span class="toggle-hint">Required components cannot be removed</span>
              </div>

              <!-- Sort Order Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Sort Order</mat-label>
                <input matInput type="number" min="0" formControlName="sortOrder" placeholder="0">
                <mat-hint>Order for displaying this component (lower numbers appear first)</mat-hint>
              </mat-form-field>
            </form>
          }
        </mat-card-content>

        @if (!loading() && !error()) {
          <mat-card-actions class="form-actions">
            <button mat-button type="button" (click)="onCancel()">Cancel</button>
            <button mat-raised-button color="primary" type="submit" [disabled]="componentForm.invalid || loading()" (click)="onSubmit()">
              @if (loading()) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                <mat-icon>{{ isEditing() ? 'save' : 'add' }}</mat-icon>
              }
              {{ isEditing() ? 'Update Component' : 'Add Component' }}
            </button>
          </mat-card-actions>
        }
      </mat-card>
    </div>
  `,
  styles: [`
    .form-container {
      padding: 24px;
      display: flex;
      justify-content: center;
      min-height: 100vh;
      background-color: #f5f5f5;
    }

    .form-card {
      max-width: 700px;
      width: 100%;
    }

    .component-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
      margin-top: 16px;
    }

    .field {
      width: 100%;
    }

    .dimensions-section {
      border: 1px solid #e0e0e0;
      border-radius: 8px;
      padding: 16px;
      background-color: #fafafa;
    }

    .dimensions-section h4 {
      margin: 0 0 8px 0;
      color: #333;
      font-weight: 500;
    }

    .dimensions-hint {
      margin: 0 0 16px 0;
      font-size: 14px;
      color: #666;
    }

    .or-divider {
      text-align: center;
      margin: 16px 0;
      position: relative;
    }

    .or-divider::before {
      content: '';
      position: absolute;
      top: 50%;
      left: 0;
      right: 0;
      height: 1px;
      background-color: #e0e0e0;
    }

    .or-divider span {
      background-color: #fafafa;
      padding: 0 16px;
      color: #666;
      font-size: 12px;
      font-weight: 500;
      text-transform: uppercase;
    }

    .fixed-length-row {
      display: flex;
      gap: 16px;
    }

    .fixed-length-row .field:first-child {
      flex: 2;
    }

    .fixed-length-row .field:last-child {
      flex: 1;
    }

    .toggle-row {
      display: flex;
      align-items: center;
      gap: 16px;
      padding: 16px;
      background-color: #fafafa;
      border-radius: 8px;
      border: 1px solid #e0e0e0;
    }

    .toggle-hint {
      font-size: 14px;
      color: #666;
    }

    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      height: 200px;
      gap: 16px;
    }

    .error-message {
      display: flex;
      align-items: center;
      gap: 12px;
      padding: 16px;
      background-color: #ffebee;
      border: 1px solid #f44336;
      border-radius: 8px;
      color: #c62828;
    }

    .form-actions {
      display: flex;
      justify-content: flex-end;
      gap: 12px;
      padding: 16px;
    }

    mat-spinner {
      margin-right: 8px;
    }

    button {
      min-width: 120px;
    }

    @media (max-width: 600px) {
      .fixed-length-row {
        flex-direction: column;
      }

      .toggle-row {
        flex-direction: column;
        align-items: flex-start;
        gap: 8px;
      }
    }
  `]
})
export class ComponentFormPage {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private state = inject(ComponentsStore);
  private snackBar = inject(MatSnackBar);

  // Reactive state from store
  components = this.state.components;
  loading = this.state.loading;
  error = this.state.error;

  // Route parameters
  isEditing = signal(false);
  editingId = signal<string | null>(null);
  systemCode = signal<string>('');

  // Form definition
  componentForm = this.fb.group({
    itemCode: new FormControl('', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    quantity: new FormControl(1, [Validators.required, Validators.min(1)]),
    description: new FormControl(''),
    lengthFormula: new FormControl(''),
    fixedLengthValue: new FormControl<number | null>(null),
    fixedLengthUnit: new FormControl('inches'),
    isRequired: new FormControl(true),
    sortOrder: new FormControl(0, [Validators.min(0)])
  });

  constructor() {
    // Get system code from route params
    const systemCode = this.route.parent?.snapshot.params['systemCode'] || '';
    this.systemCode.set(systemCode);

    // Set system code in state
    this.state.setSystemCode(systemCode);

    // Check if editing existing component
    const idParam = this.route.snapshot.params['id'];
    if (idParam) {
      this.isEditing.set(true);
      this.editingId.set(idParam);
      this.loadComponent(idParam);
    }
  }

  private loadComponent(id: string): void {
    this.state.clearError();
    // Load component data - for now, we'll get it from the list
    // In a real app, you'd have a separate getComponent action
    this.state.getComponent(id);
  }

  onSubmit() {
    if (this.componentForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.state.clearError();

    const formValue = this.componentForm.value;

    if (this.isEditing() && this.editingId()) {
      const updateRequest: UpdateComponentRequest = {
        name: formValue.name || undefined,
        quantity: formValue.quantity || undefined,
        description: formValue.description || undefined,
        lengthFormula: formValue.lengthFormula || undefined,
        fixedLengthValue: formValue.fixedLengthValue || undefined,
        fixedLengthUnit: formValue.fixedLengthUnit || undefined,
        isRequired: formValue.isRequired ?? undefined,
        sortOrder: formValue.sortOrder || undefined
      };

      this.state.updateComponentById([this.editingId()!, updateRequest]);
      this.showSuccess('Component updated successfully');
      this.navigateBack();
    } else {
      const createRequest: CreateComponentRequest = {
        itemCode: formValue.itemCode!,
        name: formValue.name!,
        quantity: formValue.quantity!,
        description: formValue.description || undefined,
        lengthFormula: formValue.lengthFormula || undefined,
        fixedLengthValue: formValue.fixedLengthValue || undefined,
        fixedLengthUnit: formValue.fixedLengthUnit || undefined,
        isRequired: formValue.isRequired ?? undefined,
        sortOrder: formValue.sortOrder || undefined
      };

      this.state.createComponent(createRequest);
      this.showSuccess('Component added successfully');
      this.navigateBack();
    }
  }

  onCancel() {
    this.navigateBack();
  }

  private showSuccess(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      horizontalPosition: 'center',
      verticalPosition: 'bottom'
    });
  }

  private navigateBack() {
    this.router.navigate(['../'], { relativeTo: this.route.parent });
  }

  private markFormGroupTouched() {
    Object.keys(this.componentForm.controls).forEach(key => {
      const control = this.componentForm.get(key);
      control?.markAsTouched();
    });
  }

  retryLoad() {
    this.state.clearError();
    if (this.isEditing() && this.editingId()) {
      this.loadComponent(this.editingId()!);
    }
  }
}
