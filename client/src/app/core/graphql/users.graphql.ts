import { Injectable } from '@angular/core';
import { Mutation, Query } from 'apollo-angular';
import gql from 'graphql-tag';

export interface User {
  id: number;
  userName: string;
  email: string;
  role: string;
  firstName?: string;
  lastName?: string;
  createdAt: Date;
  updateAt?: Date;
}

export interface Users {
  users: User[];
}

@Injectable({
  providedIn: 'root'
})
export class FindUsersGQL extends Query<Users> {
  document = gql`
    query {
      users {
        id
        userName
        email
        role
        firstName
        lastName
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class FindUserGQL extends Query<any> {
  document = gql`
    query user($id: ID) {
      user(id: $id) {
        id
        userName
        email
        role
        firstName
        lastName
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class CreateUserGQL extends Mutation {
  document = gql`
    mutation createUser($userName: String!, $email: String!, $role: String, $password: String!, $firstName: String, $lastName: String) {
      createUser(user: {
        userName: $userName
        email: $email
        role: $role
        firstName: $firstName
        lastName: $lastName
      }, password: $password) {
        id
        userName
        email
        role
        firstName
        lastName
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class UpdateUserGQL extends Mutation {
  document = gql`
    mutation updateUser($id: ID!, $userName: String!, $email: String!, $role: String, $firstName: String, $lastName: String) {
      updateUser(user: {
        id: $id
        userName: $userName
        email: $email
        role: $role
        firstName: $firstName
        lastName: $lastName
      }) {
        id
        userName
        email
        role
        firstName
        lastName
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class ChangePasswordGQL extends Mutation {
  document = gql`
    mutation changePassword($id: ID, $currentPassword: String!, $newPassword: String!) {
      changePassword(id: $id, currentPassword: $currentPassword, newPassword: $newPassword)
    }
  `;
}
