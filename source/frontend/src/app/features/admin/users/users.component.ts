import {Component, inject, Signal} from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {CommonModule} from '@angular/common';
import {AdminUserService} from '../../../services/admin-user/admin-user.service';
import {toSignal} from '@angular/core/rxjs-interop';
import {User} from '../../../models/user.models';
import {of, Subject} from 'rxjs';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule
  ],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent {
  adminUserService = inject(AdminUserService);
  users = toSignal(this.adminUserService.getUsers(), { initialValue: [] as User[] });

  updateUserRole(id: number, makeAdmin: boolean) {
    this.adminUserService.setUserRole(id, makeAdmin).subscribe(users => {
      this.users = toSignal(of(users), { initialValue: [] });
    });
  }
}
