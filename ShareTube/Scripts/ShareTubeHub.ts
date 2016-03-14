// Get signalr.d.ts.ts from https://github.com/borisyankov/DefinitelyTyped (or delete the reference)
/// <reference path="typings/signalr/signalr.d.ts" />
/// <reference path="typings/jquery/jquery.d.ts" />

////////////////////
// available hubs //
////////////////////
//#region available hubs

interface SignalR {

    /**
      * The hub implemented by ShareTube.Hubs.BaseHub
      */
    baseHub: BaseHub;

    /**
      * The hub implemented by ShareTube.Hubs.ShareTubeDBHub
      */
    shareTubeHub: ShareTubeHub;
}
//#endregion available hubs

///////////////////////
// Service Contracts //
///////////////////////
//#region service contracts

//#region BaseHub hub

interface BaseHub {
    
    /**
      * This property lets you send messages to the BaseHub hub.
      */
    server: BaseHubServer;

    /**
      * The functions on this property should be replaced if you want to receive messages from the BaseHub hub.
      */
    client: any;
}

interface BaseHubServer {
}

//#endregion BaseHub hub


//#region ShareTubeDBHub hub

interface ShareTubeHub {
    
    /**
      * This property lets you send messages to the ShareTubeDBHub hub.
      */
    server: ShareTubeDBHubServer;

    /**
      * The functions on this property should be replaced if you want to receive messages from the ShareTubeDBHub hub.
      */
    client: any;
}

interface ShareTubeDBHubServer {

    /** 
      * Sends a "server_JoinRoom" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param roomID {string} 
      * @param username {string} 
      * @return {JQueryPromise of void}
      */
    server_JoinRoom(roomID: string, username: string): JQueryPromise<void>;

    /** 
      * Sends a "server_LeaveRoom" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_LeaveRoom(): JQueryPromise<void>;

    /** 
      * Sends a "server_ReportQueue" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_ReportQueue(): JQueryPromise<void>;

    /** 
      * Sends a "server_GetQueue" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_GetQueue(): JQueryPromise<void>;

    /** 
      * Sends a "server_GetUserList" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_GetUserList(): JQueryPromise<void>;

    /** 
      * Sends a "server_GetCurrentVideo" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_GetCurrentVideo(): JQueryPromise<void>;

    /** 
      * Sends a "server_SendChatMessage" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param message {string} 
      * @return {JQueryPromise of void}
      */
    server_SendChatMessage(message: string): JQueryPromise<void>;

    /** 
      * Sends a "server_EnqueueVideo" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param url {string} 
      * @param name {string} 
      * @param author {string} 
      * @return {JQueryPromise of void}
      */
    server_EnqueueVideo(url: string, name: string, author: string): JQueryPromise<void>;

    /** 
      * Sends a "server_SelectNewVideo" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param url {string} 
      * @return {JQueryPromise of void}
      */
    server_SelectNewVideo(url: string): JQueryPromise<void>;

    /** 
      * Sends a "server_UpdatePlayerTime" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param time {number} 
      * @param sentTime {number} 
      * @return {JQueryPromise of void}
      */
    server_UpdatePlayerTime(time: number, sentTime: number): JQueryPromise<void>;

    /** 
      * Sends a "server_UpdateStatus" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param statusID {number} 
      * @return {JQueryPromise of void}
      */
    server_UpdateStatus(statusID: number): JQueryPromise<void>;

    /** 
      * Sends a "server_GetCurrentVideoUrl" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_GetCurrentVideoUrl(): JQueryPromise<void>;

    /** 
      * Sends a "server_ChangeName" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param newName {string} 
      * @return {JQueryPromise of void}
      */
    server_ChangeName(newName: string): JQueryPromise<void>;

    /** 
      * Sends a "server_NextVideo" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_NextVideo(): JQueryPromise<void>;

    /** 
      * Sends a "server_PrevVideo" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    server_PrevVideo(): JQueryPromise<void>;

    /** 
      * Sends a "server_SetLoop" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param loop {boolean} 
      * @return {JQueryPromise of void}
      */
    server_SetLoop(loop: boolean): JQueryPromise<void>;

    /** 
      * Sends a "caller_UserIsHost" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    caller_UserIsHost(): JQueryPromise<void>;

    /** 
      * Sends a "caller_GetQueue" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    caller_GetQueue(): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastChatMessage" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param message {string} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastChatMessage(message: string): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastUserJoinMessage" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param user {string} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastUserJoinMessage(user: string): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastUserLeaveMessage" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param user {string} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastUserLeaveMessage(user: string): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastVideoMessage" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param user {string} 
      * @param videoName {string} 
      * @param author {string} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastVideoMessage(user: string, videoName: string, author: string): JQueryPromise<void>;

    /** 
      * Sends a "broadcastUserList" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param roomID {Guid} 
      * @param clients {Object} 
      * @return {JQueryPromise of void}
      */
    broadcastUserList(roomID: Guid, clients: Object): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastCurrentTime" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param timeSent {number} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastCurrentTime(timeSent: number): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastPlayerStatus" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @param force {boolean} 
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastPlayerStatus(force: boolean): JQueryPromise<void>;

    /** 
      * Sends a "roomClients_BroadcastLoopStatus" message to the ShareTubeDBHub hub.
      * Contract Documentation: ---
      * @return {JQueryPromise of void}
      */
    roomClients_BroadcastLoopStatus(): JQueryPromise<void>;
}

//#endregion ShareTubeDBHub hub

//#endregion service contracts



////////////////////
// Data Contracts //
////////////////////
//#region data contracts


/**
  * Data contract for System.Object
  */
interface Object {
}


/**
  * Data contract for System.Guid
  */
interface Guid {
}

//#endregion data contracts

