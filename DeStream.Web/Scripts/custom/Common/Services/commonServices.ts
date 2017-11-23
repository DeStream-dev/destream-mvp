

module App.Common.Services {
   

    export class ModalService {
        showErrorModal(errorMessage: string) {
            var modal = $('#errorModal');
            if (modal.length) {
                if (!modal.is(':visible')) {
                    modal.find('#errorModalMessageContainer').text(errorMessage);
                    modal.modal('show');
                }
            }
            else {
                console.error('modal-danger not founded');
            }
        }

        showConfirm(title: string, message: string, okCallback: () => void) {
            var confirm = $('#confirmModal');
            if (confirm.length) {
                if (!confirm.is(':visible')) {
                    confirm.find('#confirmModalHeader').text(title);
                    confirm.find('#confirmModalMessage').text(message);
                    var okBtn = confirm.find('#confirmModalOkButton');
                    okBtn.unbind('click');
                    okBtn.click(eventObject => {
                        okCallback();
                    });
                    confirm.modal('show');
                }
            }
        }
    }

    


    export class NotificationService {
        showSuccessNotification(message: string, header?: string, hideAfterMs: number = 3000) {
            
            $.toast({
                heading: header,
                text: message,
                position: 'top-right',
                loaderBg: '#ff6849',
                icon: 'success',
                hideAfter: hideAfterMs,
                stack: 6
            });
        }
    }

    export class ErrorsService {
        getErrorMessage(err: any): string {
            var msg: string = null;
            if (err && err.data && err.data.ErrorMessage)
                msg = err.data.ErrorMessage;
            return msg;
        }
    }

    export class CollectionService {
        toRows<T>(items:T[], rowItemsCount: number):T[][] {
            var groups = [],inner;
            for (var i = 0; i < items.length; i++) {
                if (i % rowItemsCount === 0) {
                    inner = [];
                    groups.push(inner);
                }
                inner.push(items[i]);
            }
            return groups;
        }
    }
}