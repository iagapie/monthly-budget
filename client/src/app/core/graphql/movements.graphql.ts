import { Injectable } from '@angular/core';
import { Mutation, Query } from 'apollo-angular';
import gql from 'graphql-tag';

export interface Direction {
  id: number;
  name: string;
}

export interface MovementItem {
  id: number;
  movementId: number;
  date: Date;
  amount: number;
  description?: string;
  createdAt: Date;
  updateAt?: Date;
}

export interface Movement {
  id: number;
  projectId: number;
  name: string;
  planAmount: number;
  createdAt: Date;
  updateAt?: Date;
  direction: Direction;
  movementItems: MovementItem[];
}

export interface Movements {
  movements: Movement[];
}

@Injectable({
  providedIn: 'root'
})
export class FindMovementsGQL extends Query<Movements> {
  document = gql`
    query movements($projectId: ID!) {
      movements(projectId: $projectId) {
        id
        projectId
        ownerId
        name
        planAmount
        createdAt
        updatedAt
        direction {
          id
          name
        }
        movementItems {
          id
          movementId
          ownerId
          amount
          date
          description
          createdAt
          updatedAt
        }
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class CreateMovementGQL extends Mutation {
  document = gql`
    mutation createMovement($projectId: Int!, $name: String!, $planAmount: Decimal!, $directionId: ID!) {
      createMovement(movement: {
        projectId: $projectId
        name: $name
        planAmount: $planAmount
        direction: {
          id: $directionId
        }
      }) {
        id
        projectId
        ownerId
        name
        planAmount
        createdAt
        updatedAt
        direction {
          id
          name
        }
        movementItems {
          id
        }
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class UpdateMovementGQL extends Mutation {
  document = gql`
    mutation updateMovement($id: ID!, $projectId: Int!, $name: String!, $planAmount: Decimal!, $directionId: ID!) {
      updateMovement(movement: {
        id: $id
        projectId: $projectId
        name: $name
        planAmount: $planAmount
        direction: {
          id: $directionId
        }
      }) {
        id
        projectId
        ownerId
        name
        planAmount
        createdAt
        updatedAt
        direction {
          id
          name
        }
        movementItems {
          id
          movementId
          ownerId
          amount
          date
          description
          createdAt
          updatedAt
        }
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class RemoveMovementGQL extends Mutation {
  document = gql`
    mutation removeMovement($id: ID!) {
      removeMovement(id: $id) {
        id
        projectId
        ownerId
        name
        planAmount
        createdAt
        updatedAt
        direction {
          id
          name
        }
        movementItems {
          id
          movementId
          ownerId
          amount
          date
          description
          createdAt
          updatedAt
        }
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class CreateMovementItemGQL extends Mutation {
  document = gql`
    mutation createMovementItem($movementId: Int!, $date: DateTimeOffset!, $amount: Decimal!, $description: String) {
      createMovementItem(movementItem: {
        movementId: $movementId
        date: $date
        amount: $amount
        description: $description
      }) {
        id
        movementId
        ownerId
        amount
        date
        description
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class UpdateMovementItemGQL extends Mutation {
  document = gql`
    mutation updateMovementItem($id: ID!, $movementId: Int!, $date: DateTimeOffset!, $amount: Decimal!, $description: String) {
      updateMovementItem(movementItem: {
        id: $id
        movementId: $movementId
        date: $date
        amount: $amount
        description: $description
      }) {
        id
        movementId
        ownerId
        amount
        date
        description
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class RemoveMovementItemGQL extends Mutation {
  document = gql`
    mutation removeMovementItem($id: ID!) {
      removeMovementItem(id: $id) {
        id
        movementId
        ownerId
        amount
        date
        description
        createdAt
        updatedAt
      }
    }
  `;
}
