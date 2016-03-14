
declare module TwitchEmotes {
    interface API {
        meta: Meta;
        template: Template;
        emotes: any;
    }
    interface Meta {
        generated_at: string;
    }
    interface Template {
        small: string;
        medium: string;
        large: string;
    }
}
