import { Routes } from '@angular/router';
import { SystemsComponent } from './systems.component';

export const SYSTEMS_ROUTES: Routes = [
  {
    path: '',
    component: SystemsComponent,
    title: 'Systems'
  },
  {
    path: ':systemCode/components',
    loadChildren: () => import('./components').then(m => m.COMPONENTS_ROUTES)
  }
];
