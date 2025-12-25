import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

interface System {
  code: string;
  category: string;
  colors: string[];
  sizeConstraints: {
    minHeight: number;
    maxHeight: number;
    minWidth: number;
    maxWidth: number;
  };
}

@Component({
  selector: 'app-systems',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatListModule, MatIconModule],
  template: `
    <div class="systems">
      <h1 class="systems-title">Systems</h1>
      <p class="systems-subtitle">Pre-configured window and door systems</p>

      <div class="systems-grid">
        <mat-card class="system-card" *ngFor="let system of systems">
          <mat-card-header>
            <div mat-card-avatar class="system-icon">
              <mat-icon>{{ getCategoryIcon(system.category) }}</mat-icon>
            </div>
            <mat-card-title>{{ system.code }}</mat-card-title>
            <mat-card-subtitle>{{ system.category }}</mat-card-subtitle>
          </mat-card-header>

          <mat-card-content>
            <div class="system-details">
              <div class="detail-item">
                <strong>Size Range:</strong>
                {{ system.sizeConstraints.minWidth }}" - {{ system.sizeConstraints.maxWidth }}" W ×
                {{ system.sizeConstraints.minHeight }}" - {{ system.sizeConstraints.maxHeight }}" H
              </div>
              <div class="detail-item">
                <strong>Available Colors:</strong>
                {{ system.colors.join(', ') }}
              </div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .systems {
      padding: 24px;
    }

    .systems-title {
      font-size: 32px;
      font-weight: 300;
      margin-bottom: 8px;
      color: #333;
    }

    .systems-subtitle {
      font-size: 16px;
      color: #666;
      margin-bottom: 32px;
    }

    .systems-grid {
      display: grid;
      grid-template-columns: repeat(auto-fill, minmax(350px, 1fr));
      gap: 24px;
    }

    .system-card {
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
      transition: box-shadow 0.3s ease;
    }

    .system-card:hover {
      box-shadow: 0 4px 8px rgba(0,0,0,0.15);
    }

    .system-icon {
      background-color: #f5f5f5;
      display: flex;
      align-items: center;
      justify-content: center;
    }

    .system-details {
      margin-top: 16px;
    }

    .detail-item {
      margin-bottom: 12px;
      font-size: 14px;
      line-height: 1.4;
    }

    .detail-item strong {
      color: #333;
      margin-right: 8px;
    }
  `]
})
export class SystemsComponent {
  systems: System[] = [
    {
      code: 'WF00',
      category: 'Window',
      colors: ['Bronze', 'White'],
      sizeConstraints: {
        minHeight: 24,
        maxHeight: 96,
        minWidth: 24,
        maxWidth: 120
      }
    },
    {
      code: 'DF00',
      category: 'Door',
      colors: ['Bronze', 'White', 'Anodized'],
      sizeConstraints: {
        minHeight: 80,
        maxHeight: 96,
        minWidth: 30,
        maxWidth: 48
      }
    },
    {
      code: 'RF00',
      category: 'Railing',
      colors: ['Bronze', 'White'],
      sizeConstraints: {
        minHeight: 36,
        maxHeight: 42,
        minWidth: 48,
        maxWidth: 192
      }
    }
  ];

  getCategoryIcon(category: string): string {
    switch (category.toLowerCase()) {
      case 'window': return 'window';
      case 'door': return 'door_front';
      case 'railing': return 'fence';
      default: return 'build';
    }
  }
}
