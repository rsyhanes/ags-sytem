import { Component, inject, signal, computed, effect } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormControl, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { ItemsStore } from '../data-access/items.state';
import { CreateItemRequest, UpdateItemRequest } from '../data-access/items.api';
import { LoadingIndicatorComponent } from '../components/loading-indicator.component';
import { ErrorMessageComponent } from '../components/error-message.component';

type MeasureUnit = 'Units' | 'Inches';

@Component({
  selector: 'app-item-form',
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
    LoadingIndicatorComponent,
    ErrorMessageComponent
  ],
  template: `
    <div class="form-container">
      <mat-card class="form-card">
        <mat-card-header>
          <mat-card-title>{{ isEditing() ? 'Edit Item' : 'Create New Item' }}</mat-card-title>
        </mat-card-header>

        <mat-card-content>
          @if (loading(); as isLoading) {
            <app-loading-indicator />
          } @else if (error(); as errorMessage) {
            <app-error-message [message]="errorMessage" />
          } @else {
            <form [formGroup]="itemForm" (ngSubmit)="onSubmit()" class="item-form">
              <!-- Code Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Item Code</mat-label>
                <input matInput formControlName="code" placeholder="e.g. FRM001" required>
                <mat-hint>Type the code for this item</mat-hint>
                @if (itemForm.get('code')?.hasError('required')) {
                  <mat-error>Code is required</mat-error>
                }
              </mat-form-field>

              <!-- Name Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Item Name</mat-label>
                <input matInput formControlName="name" placeholder="e.g. Window Frame" required>
                <mat-hint>Enter a descriptive name</mat-hint>
                @if (itemForm.get('name')?.hasError('required')) {
                  <mat-error>Name is required</mat-error>
                }
              </mat-form-field>

              <!-- Price Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Price</mat-label>
                <input matInput type="number" step="0.01" formControlName="price" placeholder="0.00" required>
                <span matPrefix>$</span>
                @if (itemForm.get('price')?.hasError('required')) {
                  <mat-error>Price is required</mat-error>
                } @else if (itemForm.get('price')?.hasError('min')) {
                  <mat-error>Price must be positive</mat-error>
                }
              </mat-form-field>

              <!-- Measure Unit Field -->
              <mat-form-field appearance="outline" class="field">
                <mat-label>Measure Unit</mat-label>
                <mat-select formControlName="measure">
                  <mat-option value="Units">Units</mat-option>
                  <mat-option value="Inches">Inches</mat-option>
                </mat-select>
                <mat-hint>Unit of measurement for pricing</mat-hint>
              </mat-form-field>
            </form>
          }
        </mat-card-content>

        @if (!loading() && !error()) {
          <mat-card-actions class="form-actions">
            <button mat-button type="button" (click)="onCancel()">Cancel</button>
            <button mat-raised-button color="primary" type="submit" [disabled]="itemForm.invalid || loading()" (click)="onSubmit()">
              @if (loading()) {
                <mat-spinner diameter="20"></mat-spinner>
              } @else {
                <mat-icon>{{ isEditing() ? 'save' : 'add' }}</mat-icon>
              }
              {{ isEditing() ? 'Update Item' : 'Create Item' }}
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
    }

    .form-card {
      max-width: 600px;
      width: 100%;
    }

    .item-form {
      display: flex;
      flex-direction: column;
      gap: 16px;
      margin-top: 16px;
    }

    .field {
      width: 100%;
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
  `]
})
export class ItemFormPage {
  private fb = inject(FormBuilder);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private state = inject(ItemsStore);
  private snackBar = inject(MatSnackBar);

  // Reactive state from store
  items = this.state.items;
  loading = this.state.loading;
  error = this.state.error;

  // Route parameters
  isEditing = signal(false);
  editingCode = signal<string | null>(null);

  // Form definition
  itemForm = this.fb.group({
    code: new FormControl('', [Validators.required]),
    name: new FormControl('', [Validators.required]),
    price: new FormControl(0, [Validators.required, Validators.min(0)]),
    measure: new FormControl<'Units' | 'Inches'>('Units')
  });

  constructor() {
    // Check if editing existing item
    const codeParam = this.route.snapshot.params['code'];
    const isEditRoute = this.route.snapshot.url.some(segment => segment.path === 'edit');

    if (codeParam && isEditRoute) {
      this.isEditing.set(true);
      this.editingCode.set(codeParam);
      this.loadItem(codeParam);
    }
  }

  private loadItem(code: string): void {
    this.state.clearError();

    // Note: For editing, we trigger the action - the store manages loading state
    this.state.getItem(code);
  }

  onSubmit() {
    if (this.itemForm.invalid) {
      this.markFormGroupTouched();
      return;
    }

    this.state.clearError();

    const formValue = this.itemForm.value;

    if (this.isEditing() && this.editingCode()) {
      const updateRequest: UpdateItemRequest = {
        name: formValue.name || undefined,
        measure: formValue.measure || undefined
      };

      this.state.updateItemByCode([this.editingCode()!, updateRequest]);
      this.showSuccess('Item updated successfully');
      // Navigation and error handling now handled reactively by the store
      this.navigateBack();
    } else {
      const createRequest: CreateItemRequest = {
        code: formValue.code!,
        name: formValue.name!,
        description: `Item created at ${new Date().toISOString()}`,
        price: formValue.price!,
        measure: formValue.measure!
      };

      this.state.createItem(createRequest);
      this.showSuccess('Item created successfully');
      // Navigation and error handling now handled reactively by the store
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
    Object.keys(this.itemForm.controls).forEach(key => {
      const control = this.itemForm.get(key);
      control?.markAsTouched();
    });
  }
}
