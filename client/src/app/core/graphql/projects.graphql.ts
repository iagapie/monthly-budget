import { Injectable } from '@angular/core';
import { Mutation, Query } from 'apollo-angular';
import gql from 'graphql-tag';

export interface Project {
  id: number;
  name: string;
  currency: string;
  description?: string;
  createdAt: Date;
  updateAt?: Date;
}

export interface Projects {
  projects: Project[];
}

@Injectable({
  providedIn: 'root'
})
export class FindProjectsGQL extends Query<Projects> {
  document = gql`
    query {
      projects {
        id
        name
        currency
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class FindProjectGQL extends Query<any> {
  document = gql`
    query project($id: ID!) {
      project(id: $id) {
        id
        name
        currency
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class CreateProjectGQL extends Mutation {
  document = gql`
    mutation createProject($name: String!, $currency: String!) {
      createProject(project: {
        name: $name
        currency: $currency
      }) {
        id
        name
        currency
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class UpdateProjectGQL extends Mutation {
  document = gql`
    mutation updateProject($id: ID!, $name: String!, $currency: String!) {
      updateProject(project: {
        id: $id
        name: $name
        currency: $currency
      }) {
        id
        name
        currency
        createdAt
        updatedAt
      }
    }
  `;
}

@Injectable({
  providedIn: 'root'
})
export class RemoveProjectGQL extends Mutation {
  document = gql`
    mutation removeProject($id: ID!) {
      removeProject(id: $id) {
        id
        name
        currency
        createdAt
        updatedAt
      }
    }
  `;
}
