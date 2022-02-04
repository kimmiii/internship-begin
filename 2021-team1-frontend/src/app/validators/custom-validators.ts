import { FormArray } from '@angular/forms';

export class CustomValidators {
  // validator to check if at least one checkbox is checked
  static multipleCheckboxRequireOne(fa: FormArray) {
    let valid = false;

    for (let x = 0; x < fa.length; ++x) {
      if (fa.at(x).value) {
        valid = true;
        break;
      }
    }
    return valid ? null : {
      multipleCheckboxRequireOne: true
    };
  }
}
