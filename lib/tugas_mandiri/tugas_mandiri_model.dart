class Coffe {
  int? id;
  String? title;
  String? description;
  double? price;
  String? image;
  int? totalSales;
  List<String>? ingredients;
  Coffe({
    this.id,
    this.title,
    this.description,
    this.price,
    this.image,
    this.totalSales,
  });
  Coffe.fromJson(Map<String, dynamic> json) {
    id = json['id'] as int?;
    title = json['title'];
    description = json['description'];
    price = json['price'];
    image = json['image'];
    totalSales = json['totalSales'];
    ingredients = json['ingredients'] != null
        ? List<String>.from(json['ingredients'])
        : null;
  }
}
