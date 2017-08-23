app.service("APIService", function ($http) {
    this.getBooks = function () {
        return $http.get("/api/books")
    };


    this.getBookDetail = function (id) {
        return $http.get('/api/Books/' + id)

    
    };



});