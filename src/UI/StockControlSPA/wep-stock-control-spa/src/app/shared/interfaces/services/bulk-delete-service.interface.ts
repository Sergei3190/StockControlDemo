import { Guid } from "guid-ts";
import { IBulkDeleteResult } from "../bulk-delete-result.interface";
import { Observable } from "rxjs";
import { IError } from "../error.interface";

export interface IBulkDeleteService {
    bulkDelete: (ids: Guid[]) => Observable<IBulkDeleteResult | IError>
}