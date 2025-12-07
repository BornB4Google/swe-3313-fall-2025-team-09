import { Component, inject, Signal } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdminUserService } from '../../../services/admin-user/admin-user.service';
import { toSignal } from '@angular/core/rxjs-interop';
import { User } from '../../../models/user.models';
import { Subject, startWith, switchMap } from 'rxjs';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent {
  adminUserService = inject(AdminUserService);
  private readonly refreshTrigger$ = new Subject<void>();
  users: Signal<User[]> = toSignal(
    this.refreshTrigger$.pipe(
      startWith<void>(undefined),
      switchMap(() => this.adminUserService.getUsers())
    ),
    { initialValue: [] as User[] }
  );

  updateUserRole(id: number, makeAdmin: boolean) {
    this.adminUserService.setUserRole(id, makeAdmin).subscribe(() => {
      this.refreshTrigger$.next();
    });
  }
}
