import { Movement, MovementItem } from '../../core/graphql/movements.graphql';

export class Calc {
  // private movements: {[id: number]: {actual: number, diff: number}} = {};

  static diffFn(direction: number): (plan: number, actual: number) => number {
    return +direction === 1 ? (plan, actual) => actual - plan : (plan, actual) => plan - actual;
  }

  static total(items: MovementItem[]): number {
    return items.map(x => x.amount).reduce((a, b) => a + b, 0);
  }

  static calc(movement: Movement): {actual: number, diff: number} {
    const actual = Calc.total(movement.movementItems);
    return {actual, diff: Calc.diffFn(movement.direction.id)(movement.planAmount, actual)};
  }

  static actual(movement: Movement): number {
    return Calc.calc(movement).actual;
  }

  static diff(movement: Movement): number {
    return Calc.calc(movement).diff;
  }
}
