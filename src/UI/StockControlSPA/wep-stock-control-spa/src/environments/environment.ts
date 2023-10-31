declare let window: any;

export const environment = {
    production: false,
    apiBaseUri: window["env"]["apiBaseUri"] || "//localhost:7001",
    useHash: window["env"]["useHash"] || false
};
