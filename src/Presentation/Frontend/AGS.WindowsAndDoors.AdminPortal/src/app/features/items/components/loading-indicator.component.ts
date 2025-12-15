import { Component } from '@angular/core';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-loading-indicator',
  standalone: true,
  imports: [MatProgressSpinnerModule],
  template: `
    <div class="loading-container">
      <mat-spinner diameter="40"></mat-spinner>
      <span>Loading...</span>
    </div>
  `,
  styles: [`
    .loading-container {
      display: flex;
      flex-direction: column;
      align-items: center;
      justify-content: center;
      padding: 20px;
      gap: 12px;
    }

    mat-spinner {
      margin: 0;
    }

    span {
      font-size: 14px;
      color: var(--mat-sys-on-surface);
    }
  `]
})
export class LoadingIndicatorComponent { }
