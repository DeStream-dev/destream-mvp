interface JQueryStatic {
    toast(toastObject: ToastObject);
}

interface ToastObject {
    heading: string;
    text: string;
    position: string;
    loaderBg: string;
    icon: string;
    hideAfter: number;
    stack: number;
}