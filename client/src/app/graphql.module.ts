import { NgModule } from '@angular/core';
import { ApolloModule, Apollo } from 'apollo-angular';
import { HttpLinkModule, HttpLink } from 'apollo-angular-link-http';
import { InMemoryCache } from 'apollo-cache-inmemory';
import { environment } from '../environments/environment';
import { ApolloLink, from } from 'apollo-link';
import { TokenService } from './core/services/token.service';
import { CommonModule } from '@angular/common';
import { AuthenticationService } from './core/services/authentication.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { setContext } from 'apollo-link-context';

@NgModule({
  imports: [
    CommonModule,
    ApolloModule,
    HttpLinkModule
  ],
  exports: [ApolloModule, HttpLinkModule]
})
export class GraphQLModule {
  constructor(
    private apollo: Apollo,
    private httpLink: HttpLink,
    private tokenService: TokenService,
    private authService: AuthenticationService,
    private jwtHelper: JwtHelperService
  ) {
    const http = httpLink.create({
      uri: environment.graphql_url
    });

    const authorization = new ApolloLink((operation, forward) => {
      operation.setContext(({headers}) => ({
        ...headers,
        headers: {
          Authorization: this.tokenService.access ? `Bearer ${this.tokenService.access}` : null
        }
      }));

      return forward(operation);
    });

    const reAuth = setContext(() => {
      if (this.jwtHelper.isTokenExpired(this.tokenService.access)) {
        return this.authService.refreshToken().toPromise().then(() => {
          return {
            headers: {
              Authorization: this.tokenService.access ? `Bearer ${this.tokenService.access}` : null
            }
          };
        });
      }

      return {
        headers: {
          Authorization: this.tokenService.access ? `Bearer ${this.tokenService.access}` : null
        }
      };
    });

    apollo.create({
      link: from([authorization, reAuth, http]),
      cache: new InMemoryCache()
    });
  }
}
