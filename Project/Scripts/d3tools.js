function DrawCircleImage(userid, images, imagesScores) {


    var margin = { top: 10, right: 5, bottom: 10, left: 5 };

    var svg = d3.select("#happiest-div")
        .append("svg")
        .attr("width", "100%")
        .attr("height", 500)
        .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var defs = svg.append('svg:defs');
    var config = { "avatar_size": 450 };

    var currentX = 150;
    var currentY = 70;

    

    for (var v in images) {
        var score = imagesScores[v]*100;
        var imageid = images[v].photoId;
        var patternId = "ptr_img_" + imageid;
        console.log(images[v]);

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
            .attr("y", 0)
            .attr("viewBox", "0 0 450 450")
            .attr("preserveAspectRatio", "xMinYMin meet");

        svg.append("circle")
        .style("stroke", "gray")
        .style("fill", "url(#" + patternId + ")")
        .attr("r", 120)
        .attr("cx", currentX)
        .attr("cy", 130)
        .attr("width", 300)
        .attr("height", 300);

        var group = svg.append("g");
        var rect = group.append("rect")
        .attr("x", currentX - 20)
        .attr("y", 200)
        .attr("rx", 10)
        .attr("ry", 12)
        .style("fill", "gray")
        .style("fill-opacity", 0.4)
        .attr("width", 60)
        .attr("height", 35)
        group.append("text")
        .text("%" + score.toFixed(2))
        .style("stroke", "black")
        .attr("x", currentX - 15)
        .attr("y", 220);

        currentX += 250;
    }
    

    
}

function alo() {
    alert();
}