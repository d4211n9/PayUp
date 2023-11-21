import {Injectable, numberAttribute} from '@angular/core';
import {HttpClient} from "@angular/common/http";

export interface InvitableUser {
  id: number
  full_name: string
  profile_url: string
}

export interface Pagination {
  current_page: number
  page_size: number
}

export interface PaginationResponse<T> {
  total_pages: number
  values: T
}

@Injectable()
export class UserService {

  constructor(private readonly http: HttpClient) { }

  get_invitable_users(search_query: string, pagination: Pagination) {
    return this.http.get<PaginationResponse<InvitableUser[]>>('http://localhost:5100/api/user/search?searchquery=' + search_query + '&currentpage=' + pagination.current_page + '&pagesize=' + pagination.page_size);
  }
}
