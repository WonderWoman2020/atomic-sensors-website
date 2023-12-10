
export class DataStore {
    static setItem(key: DataStoreKey, data: any) {
        localStorage.setItem(key, JSON.stringify(data));
    }

    static getItem(key: DataStoreKey) {
        const checkItem = localStorage.getItem(key)
        if (checkItem) {
            return JSON.parse(localStorage.getItem(key) || "{}");
        }
        return undefined
    }

    static removeItem(key: DataStoreKey) {
        localStorage.removeItem(key);
    }
}

export enum DataStoreKey {
    NULL = "NULL"
}

export class DataStoreEvent {
    name: string;
    value: any;

    constructor(name: string, value: any) {
        this.name = name;
        this.value = value;
    }
}