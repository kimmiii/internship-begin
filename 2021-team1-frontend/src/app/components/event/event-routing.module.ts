import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { RoleGuard } from '../../guards/role.guard';
import { RoleCode } from '../../models/role.model';

const routes: Routes = [
  {
    path: 'coordinator',
    canActivate: [RoleGuard],
    data: { role: RoleCode.COO },
    loadChildren: () =>
      import('./coordinator/coordinator.module').then(
        (m) => m.CoordinatorModule,
      ),
  },
  {
    path: 'company',
    canActivate: [RoleGuard],
    data: { role: RoleCode.COM },
    loadChildren: () =>
      import('./company/company.module').then(
        (m) => m.CompanyModule,
      ),
  },
  {
    path: 'student',
    canActivate: [RoleGuard],
    data: { role: RoleCode.STU },
    loadChildren: () =>
      import('./student/student.module').then(
        (m) => m.StudentModule,
      ),
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class EventRoutingModule {}
