import TableContainer from "./TableContainer.js";
import Actions from "./Actions.js";

const table = new TableContainer("#user-data-table", "#select-all-icon");

const actions = new Actions(table, "#block-users-btn", "#delete-users-btn", "#unblock-users-btn");