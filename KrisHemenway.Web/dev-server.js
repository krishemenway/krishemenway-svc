const express = require('express')
const request = require('request')
const app = express()
const port = 3000

app.use("/api", function(req, res) {
    var url = 'https://www.krishemenway.com/api' + req.url
    var r = null

    if (req.method === 'POST') {
       r = request.post({uri: url, json: req.body})
    } else {
       r = request(url)
    }
  
    req.pipe(r).pipe(res)
})
app.use(express.static("dist"))
app.listen(port, () => console.log(`Dev Server started on port ${port}!`))