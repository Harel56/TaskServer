import { ChangeDetectionStrategy, Component, computed, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { AuthService } from '../../services/auth.service';
import { Router, RouterLink } from '@angular/router';
import { tap } from 'rxjs';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-register',
  imports: [RouterLink, FormsModule, MatFormFieldModule, MatButtonModule, MatInputModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class RegisterComponent {

  authService = inject(AuthService);
  router = inject(Router);

  username = signal('');
  password = signal('');
  passwordAgain = signal('');

  sending = signal(false);

  passwordsMatch = computed(() => !this.password() || !this.passwordAgain() || this.password() === this.passwordAgain());

  onSubmit() {
    if (this.username() && this.password() && this.passwordAgain() && this.passwordsMatch()) {
      console.log('Form submitted', this.username(), this.password(), this.passwordAgain());
      this.authService.register(this.username(), this.password()).pipe(
        tap({next: () => this.sending.set(true), finalize: () => this.sending.set(false)}),
      ).subscribe({
        next: () => {
          this.router.navigate(['/login']);
        },
        error: () => {
          alert('Error creating user');
        }
      });
    }
  }

}