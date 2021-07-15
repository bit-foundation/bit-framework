class Bit {
    static listOfAbortControllers: BitAbortController[] = [];

    static setProperty(element: { [key: string]: any }, property: string, value: any): void {
        element[property] = value;
    }

    static getProperty(element: { [key: string]: any }, property: string): string | null {
        return element[property];
    }

    static getBoundingClientRect(element: any): object {
        return element.getBoundingClientRect();
    }
    static onWindowMouseUp(dotnetHelper: any, callback: string): string {
        const controller = new BitAbortController();

        var listenerOptions = new BitEventListenerOptions();
        listenerOptions.signal = controller.abortController.signal;

        document.addEventListener('mouseup', (e) => {

            var eventArgs = Bit.toMouseEventArgsMapper(e);

            dotnetHelper.invokeMethodAsync(callback, eventArgs);
        }, listenerOptions);

        Bit.listOfAbortControllers.push(controller)

        return controller.id;
    }
    static onWindowMouseMove(dotnetHelper: any, callback: string): string {
        const controller = new BitAbortController();

        var listenerOptions = new BitEventListenerOptions();
        listenerOptions.signal = controller.abortController.signal;

        document.addEventListener('mousemove', (e) => {

            var eventArgs = Bit.toMouseEventArgsMapper(e);

            dotnetHelper.invokeMethodAsync(callback, eventArgs);
        }, listenerOptions);

        Bit.listOfAbortControllers.push(controller)

        return controller.id;
    }

    static toMouseEventArgsMapper(e: MouseEvent): object {

        return {
            altKey: e.altKey,
            button: e.button,
            buttons: e.buttons,
            clientX: e.clientX,
            clientY: e.clientY,
            ctrlKey: e.ctrlKey,
            detail: e.detail,
            metaKey: e.metaKey,
            offsetX: e.offsetX,
            offsetY: e.offsetY,
            screenY: e.screenY,
            screenX: e.screenX,
            shiftKey: e.shiftKey,
            type: e.type,
        };
    }

    static abortProcedure(id: string): void {
        var aborController = Bit.listOfAbortControllers.find(ac => ac.id == id)?.abortController;

        if (aborController) {
            aborController.abort();
        }
    }
}

class BitEventListenerOptions implements EventListenerOptions {
    capture?: boolean;
    signal?: AbortSignal;
}

class BitAbortController {
    id: string = Math.random().toString(36).substr(2, 9);
    abortController = new AbortController();
}