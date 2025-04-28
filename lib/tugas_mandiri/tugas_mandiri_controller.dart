import 'dart:convert';
import 'package:mobile/tugas_mandiri/tugas_mandiri_model.dart';
import 'package:http/http.dart' as http;

class CoffeController {
  static Future<List<Coffe>> fetchAllCoffe() async {
    final response =
        await http.get(Uri.parse('https://api.sampleapis.com/coffee/iced'));

    if (response.statusCode == 200) {
      final List<dynamic> jsonResponse = json.decode(response.body);
      return jsonResponse.map((json) => Coffe.fromJson(json)).toList();
    } else {
      throw Exception('Failed to load coffee');
    }
  }
}
