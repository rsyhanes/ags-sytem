import { Component, input } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-error-message',
  standalone: true,
  imports: [MatIconModule],
  template: `
    <div class="error-container" role="alert" aria-live="polite">
      <mat-icon class="error-icon">error_outline</mat-icon>
      <span class="error-text">{{ message() }}</span>
    </div>
  `,
  styles: [`
    .error-container {
      display: flex;
      align-items: center;
      gap: 8px;
      padding: 12px 16px;
      background-color: var(--mat-sys-error-container);
      border-radius: 8px;
      border-left: 4px solid var(--mat-sys-error);
    }

    .error-icon {
      color: var(--mat-sys-error);
      font-size: 20px;
      width: 20px;
      height: 20px;
    }

    .error-text {
      color: var(--mat-sys-error);
      font-size: 14px;
      line-height: 1.4;
    }
  `]
})
export class ErrorMessageComponent {
  message = input.required<string>();
}
