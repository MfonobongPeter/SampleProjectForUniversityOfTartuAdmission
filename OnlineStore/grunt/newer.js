module.exports = {
    newer: {
        options: {
            override: function (detail, include) {
                if (detail.task === 'compass') {
                    checkForModifiedImports(detail.path, detail.time, include);
                } else {
                    include(false);
                }
            }
        }
    }
}