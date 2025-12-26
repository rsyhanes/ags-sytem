import { Routes } from '@angular/router';
import { ComponentsListPage } from './pages/components-list.page';
import { ComponentFormPage } from './pages/component-form.page';

const COMPONENTS_ROUTES: Routes = [
  {
    path: '',
    component: ComponentsListPage,
    title: 'System Components'
  },
  {
    path: 'new',
    component: ComponentFormPage,
    title: 'Add Component'
  },
  {
    path: ':id/edit',
    component: ComponentFormPage,
    title: 'Edit Component'
  }
];

export { COMPONENTS_ROUTES };
