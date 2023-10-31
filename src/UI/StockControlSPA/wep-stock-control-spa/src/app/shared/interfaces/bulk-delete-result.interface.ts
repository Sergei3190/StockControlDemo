import { IBulkDeleteSuccessMessage } from "./bulk-delete-success-message.interface";

export interface IBulkDeleteResult {
    successMessage?: IBulkDeleteSuccessMessage; 

    errorMessage?: string[];
}