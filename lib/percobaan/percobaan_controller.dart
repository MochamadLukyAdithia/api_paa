import 'dart:convert';
import 'package:mobile/percobaan/percobaan_model.dart';
import 'package:http/http.dart' as http;

class ProdukController {
  static Future<List<Produk>> fetchAllProduk() async {
    final response =
        await http.get(Uri.parse('https://dummyjson.com/products'));
    if (response.statusCode == 200) {
      final Map<String, dynamic> jsonResponse = json.decode(response.body);
      final List<dynamic> produkList = jsonResponse['products'];
      return produkList.map((json) => Produk.fromJson(json)).toList();
    } else {
      throw Exception('Failed to load produk');
    }
  }
}
