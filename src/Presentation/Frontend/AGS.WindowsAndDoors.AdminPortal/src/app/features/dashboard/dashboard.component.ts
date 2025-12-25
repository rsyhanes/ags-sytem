import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

interface MetricCard {
  title: string;
  value: string;
  icon: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <div class="dashboard">
      <h1 class="dashboard-title">Dashboard</h1>

      <div class="metrics-grid">
        <mat-card class="metric-card" *ngFor="let metric of metrics">
          <mat-card-content class="metric-content">
            <div class="metric-icon" [style.color]="metric.color">
              <mat-icon>{{ metric.icon }}</mat-icon>
            </div>
            <div class="metric-data">
              <div class="metric-value">{{ metric.value }}</div>
              <div class="metric-title">{{ metric.title }}</div>
            </div>
          </mat-card-content>
        </mat-card>
      </div>

      <div class="recent-activity">
        <mat-card class="activity-card">
          <mat-card-header>
            <mat-card-title>Recent Orders</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <div class="activity-item">
              <mat-icon>shopping_cart</mat-icon>
              <span>Order #1234 - Window System WF00</span>
              <span class="activity-time">2 hours ago</span>
            </div>
            <div class="activity-item">
              <mat-icon>shopping_cart</mat-icon>
              <span>Order #1235 - Door System DF00</span>
              <span class="activity-time">4 hours ago</span>
            </div>
            <div class="activity-item">
              <mat-icon>shopping_cart</mat-icon>
              <span>Order #1236 - Railing System RF00</span>
              <span class="activity-time">1 day ago</span>
            </div>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard {
      padding: 24px;
    }

    .dashboard-title {
      font-size: 32px;
      font-weight: 300;
      margin-bottom: 32px;
      color: #333;
    }

    .metrics-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 24px;
      margin-bottom: 32px;
    }

    .metric-card {
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .metric-content {
      display: flex;
      align-items: center;
      padding: 24px;
    }

    .metric-icon {
      margin-right: 16px;
      font-size: 48px;
      width: 48px;
      height: 48px;
    }

    .metric-data {
      flex: 1;
    }

    .metric-value {
      font-size: 32px;
      font-weight: 500;
      line-height: 1;
      margin-bottom: 4px;
    }

    .metric-title {
      font-size: 14px;
      color: #666;
      text-transform: uppercase;
      letter-spacing: 0.5px;
    }

    .recent-activity {
      margin-top: 32px;
    }

    .activity-card {
      border-radius: 8px;
      box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .activity-item {
      display: flex;
      align-items: center;
      padding: 12px 0;
      border-bottom: 1px solid #f0f0f0;
    }

    .activity-item:last-child {
      border-bottom: none;
    }

    .activity-item mat-icon {
      margin-right: 12px;
      color: #666;
    }

    .activity-item span {
      flex: 1;
      font-size: 14px;
    }

    .activity-time {
      font-size: 12px;
      color: #999;
      margin-left: auto;
    }
  `]
})
export class DashboardComponent {
  metrics: MetricCard[] = [
    {
      title: 'Total Orders',
      value: '156',
      icon: 'shopping_cart',
      color: '#2196f3'
    },
    {
      title: 'Pending Orders',
      value: '23',
      icon: 'schedule',
      color: '#ff9800'
    },
    {
      title: 'Active Systems',
      value: '12',
      icon: 'build',
      color: '#4caf50'
    },
    {
      title: 'Total Items',
      value: '1,247',
      icon: 'inventory',
      color: '#9c27b0'
    }
  ];
}
