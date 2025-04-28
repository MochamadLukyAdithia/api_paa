import 'package:flutter/material.dart';
import 'package:mobile/tugas_mandiri/tugas_mandiri_model.dart';

class CoffeDetailView extends StatelessWidget {
  final Coffe snapshot;
  const CoffeDetailView({super.key, required this.snapshot});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      appBar: AppBar(
        title: Text(
          snapshot.title ?? "Detail Coffee",
          style: TextStyle(
            fontSize: 24,
          ),
        ),
        centerTitle: true,
        backgroundColor: Colors.brown[400],
      ),
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            snapshot.image != null && snapshot.image!.isNotEmpty
                ? Image.network(
                    snapshot.image!,
                    width: double.infinity,
                    height: MediaQuery.of(context).size.height / 2,
                    fit: BoxFit.cover,
                  )
                : Container(
                    width: double.infinity,
                    height: MediaQuery.of(context).size.height / 2,
                    color: Colors.grey[300],
                    child: const Icon(
                      Icons.image_not_supported,
                      size: 100,
                      color: Colors.grey,
                    ),
                  ),
            Container(
              padding: const EdgeInsets.all(20),
              decoration: BoxDecoration(
                color: Colors.white,
                borderRadius: const BorderRadius.only(
                  topLeft: Radius.circular(24),
                  topRight: Radius.circular(24),
                ),
              ),
              transform: Matrix4.translationValues(0, -20, 0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Text(
                    snapshot.title ?? "No Title",
                    style: const TextStyle(
                      fontSize: 28,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  const SizedBox(height: 10),
                  Row(
                    children: [
                      Text(
                        snapshot.price != null
                            ? "\$ ${snapshot.price}"
                            : "No Price",
                        style: TextStyle(
                          fontSize: 20,
                          fontWeight: FontWeight.bold,
                          color: Colors.brown[600],
                        ),
                      ),
                      const SizedBox(width: 16),
                      Icon(Icons.shopping_cart,
                          color: Colors.brown[400], size: 20),
                      const SizedBox(width: 4),
                      Text(
                        "${snapshot.totalSales ?? 0} sold",
                        style: const TextStyle(fontSize: 16),
                      ),
                    ],
                  ),
                  const SizedBox(height: 20),
                  const Text(
                    "Description",
                    style: TextStyle(
                      fontSize: 20,
                      fontWeight: FontWeight.w600,
                    ),
                  ),
                  const SizedBox(height: 8),
                  Text(
                    snapshot.description ?? "No description available.",
                    style: const TextStyle(
                      fontSize: 16,
                      height: 1.5,
                    ),
                    textAlign: TextAlign.justify,
                  ),
                  const SizedBox(height: 20),
                  if (snapshot.ingredients != null &&
                      snapshot.ingredients!.isNotEmpty)
                    const Text(
                      "Ingredients",
                      style: TextStyle(
                        fontSize: 20,
                        fontWeight: FontWeight.w600,
                      ),
                    ),
                  SizedBox(height: 8),
                  if (snapshot.ingredients != null &&
                      snapshot.ingredients!.isNotEmpty)
                    Wrap(
                      spacing: 8,
                      runSpacing: 8,
                      children: snapshot.ingredients!
                          .map(
                            (ingredient) => Chip(
                              label: Text(ingredient),
                              backgroundColor: Colors.brown[100],
                              labelStyle: const TextStyle(fontSize: 14),
                            ),
                          )
                          .toList(),
                    ),
                ],
              ),
            ),
          ],
        ),
      ),
    );
  }
}
