import { ILoadingInfo } from "../interfaces/loading-info.interface";

export class LoadingInfoModel implements ILoadingInfo {
    isResultFilter = false;
    isLoading = false;
    isNotFound = false;
    isNoContent = false;
    isUnauthorized = false;
}