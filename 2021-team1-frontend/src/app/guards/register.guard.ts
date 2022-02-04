import {Injectable} from "@angular/core";
import {CanDeactivate} from "@angular/router";
import {RegisterComponent} from "../components/general/register/register.component";

@Injectable()
export class RegisterGuard implements CanDeactivate<RegisterComponent> {

  canDeactivate(component: RegisterComponent): boolean {
    if (component.registerForm.dirty) {
      return window.confirm('Bent u zeker dat u dit venster wilt verlaten? De ingevulde gegevens gaan verloren.');
    }

    return true;
  }
}
