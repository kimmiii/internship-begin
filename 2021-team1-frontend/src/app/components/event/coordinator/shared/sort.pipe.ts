import { Pipe, PipeTransform } from '@angular/core';
import { Sort } from '@angular/material';
import { cloneDeep } from 'lodash';

@Pipe({
  name: 'sort',
})
export class SortPipe implements PipeTransform {

  transform<T>(items: ReadonlyArray<T>, sortBy: (items: ReadonlyArray<T>) => T[], sort: Sort): ReadonlyArray<T> {
    if (sort && sort.direction) {
      return sort.direction === 'asc'
        ? sortBy(cloneDeep(items))
        : sortBy(cloneDeep(items)).reverse();
    }

    return cloneDeep(items);
  }

}
