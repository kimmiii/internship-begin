import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { NotFoundComponent } from '../components/exception/not-found/not-found.component';
import { ServerErrorComponent } from '../components/exception/server-error/server-error.component';
import { LoginComponent } from '../components/general/login/login.component';
import { RegisterSuccessfulComponent } from '../components/general/register-successful/register-successful.component';
import { RegisterComponent } from '../components/general/register/register.component';
import { ContainerComponent } from '../container.component';
import { LoggedInGuard } from '../guards/logged-in.guard';
import { RegisterGuard } from '../guards/register.guard';
import { RoleGuard } from '../guards/role.guard';
import { RoleCode } from '../models/role.model';

const routes: Routes = [
  {
    path: '',
    component: ContainerComponent,
    canActivate: [LoggedInGuard],
    children: [
      {
        path: 'company',
        canActivate: [RoleGuard],
        data: { role: RoleCode.COM },
        loadChildren: () =>
          import('../components/company/company.module').then(
            (m) => m.CompanyModule,
          ),
      },
      {
        path: 'coordinator',
        canActivate: [RoleGuard],
        data: { role: RoleCode.COO },
        loadChildren: () =>
          import('../components/coordinator/coordinator.module').then(
            (m) => m.CoordinatorModule,
          ),
      },
      {
        path: 'reviewer',
        canActivate: [RoleGuard],
        data: { role: RoleCode.REV },
        loadChildren: () =>
          import('../components/reviewer/reviewer.module').then(
            (m) => m.ReviewerModule,
          ),
      },
      {
        path: 'student',
        canActivate: [RoleGuard],
        data: { role: RoleCode.STU },
        loadChildren: () =>
          import('../components/student/student.module').then(
            (m) => m.StudentModule,
          ),
      },
      {
        path: 'event',
        loadChildren: () =>
          import('../components/event/event.module').then((m) => m.EventModule),
      },
    ],
  },
  { path: 'login', component: LoginComponent },
  {
    path: 'register',
    component: RegisterComponent,
    canDeactivate: [RegisterGuard],
  },
  {
    path: 'register-successful',
    component: RegisterSuccessfulComponent,
  },
  { path: 'server-error', component: ServerErrorComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: '**', component: NotFoundComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: true })],
  exports: [RouterModule],
})
export class AppRoutingModule {}
