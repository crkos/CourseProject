import TableContainer from "./TableContainer.js";

export default class Actions {
    /** @type {TableContainer} */
    _tableInstance;
    _blockBtn;
    _deleteBtn;
    _unblockBtn;

    /**
     * Constructor
     * @param {TableContainer} tableContainer - Instance of TableContainer
     * @param {string} blockBtnId
     * @param {string} deleteBtnId
     * @param {string} unblockBtnId
     */
    constructor(tableContainer, blockBtnId, deleteBtnId, unblockBtnId) {
        this._tableInstance = tableContainer
        this._blockBtn = $(blockBtnId)
        this._deleteBtn = $(deleteBtnId)
        this._unblockBtn = $(unblockBtnId)

        this.setStatus = this.setStatus.bind(this);
        this.deleteUsers = this.deleteUsers.bind(this);

        this._blockBtn.click(this.setStatus);
        this._unblockBtn.click(this.setStatus);
        this._deleteBtn.click(this.deleteUsers);
    }

    /**
     * 
     *
     * @param {SubmitEvent} action
     * @returns
     */
    setStatus(action) {
        const userIds = this._tableInstance.getValues();

        const button = $(action.currentTarget);
        const status = button.attr('value');


        if (userIds.length === 0) {
            toastr.warning("No users were selected.");
            return
        }

        const data = {
            userIds,
            status
        }


        $.ajax({
            url: `/Account/SetStatus`,
            type: "PATCH",
            contentType: "application/json",
            data: JSON.stringify(data),
            success: function (response) {
                toastr.success("Updated user successfully!");
                setTimeout(() => {
                    location.reload();
                }, 1000)
            },
            error: function (xhr, status, error) {
                toastr.error("There was an error while updating this error, try again!");
            }
        })
    }

    deleteUsers() {
        const userIds = this._tableInstance.getValues();

        if (userIds.length === 0) {
            toastr.warning("No users were selected.");
            return
        }

        $.ajax({
            url: `/Account/Delete`,
            type: "DELETE",
            contentType: "application/json",
            data: JSON.stringify(userIds),
            success: function (response) {
                toastr.success("Users deleted!");
                setTimeout(() => {
                    location.reload();
                }, 1000)
            },
            error: function (xhr, status, error) {
                toastr.error("Something went wrong while deleting the user, try again!");
            }
        })
    }

}