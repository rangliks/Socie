function DrawCircleImage(userid, images) {
    var svg = d3.select("#happiest-div")
        .append("svg")
        .attr("width", 1000)
        .attr("height", 500);

    var defs = svg.append('svg:defs');
    var config = { "avatar_size": 250 };

    var currentX = 150;
    var currentY = 70;
    for (var v in images) {
        var imageid = images[v].photoId;
        var patternId = "ptr_img_" + imageid;
        console.log("--------");
        console.log(imageid + " ==== " + patternId);
        console.log("--------");

        defs.append("svg:pattern")
            .attr("id", patternId)
            .attr("width", config.avatar_size)
            .attr("height", config.avatar_size)
            .attr("patternUnits", "userSpaceOnUse")
            .append("svg:image")
            .attr("xlink:href", '/Content/images/' + userid + "/image_" + imageid + ".jpg")
            .attr("width", config.avatar_size)
            .attr("height", config.avatar_size)
            .attr("x", 0)
            .attr("y", -40);

        svg.append("circle")
        .style("stroke", "gray")
        .style("fill", "url(#" + patternId + ")")
        .attr("r", 100)
        .attr("cx", currentX)
        .attr("cy", 100);

        currentX += 180;
    }
    

    
}

function alo() {
    alert();
}