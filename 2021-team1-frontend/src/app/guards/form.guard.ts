import {Injectable} from "@angular/core";
import {CanDeactivate} from "@angular/router";

@Injectable()
export class FormGuard implements CanDeactivate<any> {

  canDeactivate(component): boolean {
    if (component.applicationForm != null && component.applicationForm.dirty) {
      return window.confirm('Bent u zeker dat u dit venster wilt verlaten? De ingevulde gegevens gaan verloren.');
    }

    return true;
  }
}
